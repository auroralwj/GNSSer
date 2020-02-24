//2014.05.29, czs, added, 参考 projnet 之 StreamTokenizer。

using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Geo.Common.IO
{
    ///<summary>
    ///本类将输入流以指定的字符（令牌、标记、记号等）进行解析，每一次读取一个令牌。
    ///本类可以识别令牌包括：数字、单词、引号中的字符串。
    ///</summary>
    ///<remarks>
    ///
    /// 这是参照Java StreamTokenizer的实现。
    ///</remarks>
    public class StreamTokenizer
    {       
       protected  TokenType _currentTokenType;
       protected TextReader _reader;
       protected string _currentToken;
       protected bool _ignoreWhitespace = false;
       protected int _lineNumber = 1;
       protected int _colNumber = 1;

        #region Constructors


        /// <summary>
        /// 初始化一个 StreamTokenizer 类实例。
        /// </summary>
        /// <param name="reader">A TextReader with some text to read.</param>
        /// <param name="ignoreWhitespace">Flag indicating whether whitespace should be ignored.</param>
        public StreamTokenizer(TextReader reader, bool ignoreWhitespace)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            _reader = reader;
            _ignoreWhitespace = ignoreWhitespace;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 当前行的编号，从 1 开始。
        /// </summary>
        public int LineNumber { get { return _lineNumber; } }
        /// <summary>
        /// 当前行的列号（即令牌号），从 1 开始。
        /// </summary>
        public int Column { get { return _colNumber; } }


        #endregion

        #region Methods

        /// <summary>
        /// 读取一个令牌，并检查是否是期望中的类型，如果不是则报错。
        /// </summary>
        /// <param name="expectedToken">The expected token.</param>
        public void ReadToken(string expectedToken)
        {
            this.NextToken();
            if (this.GetStringValue() != expectedToken)
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture.NumberFormat, "Expecting ('{3}') but got a '{0}' at line {1} column {2}.", this.GetStringValue(), this.LineNumber, this.Column, expectedToken));
            }
        }
        /// <summary>
        /// 读取双引号中的字符串。
        /// </summary>
        /// <remarks>
        /// White space inside quotes is preserved.
        /// </remarks>
        /// <returns>The string inside the double quotes.</returns>
        public string ReadDoubleQuotedWord()
        {
            string word = "";
            ReadToken("\"");
            NextToken(false);
            while (GetStringValue() != "\"")
            {
                word = word + this.GetStringValue();
                NextToken(false);
            }
            return word;
        }

        /// <summary>
        /// 若当前令牌为数字类型，则返回该数值。
        /// </summary>
        /// <exception cref="FormatException">Current token is not a number in a valid format.</exception>
        public double GetNumericValue()
        {
            string number = this.GetStringValue();
            if (this.GetTokenType() == TokenType.Number)
                return double.Parse(number, CultureInfo.InvariantCulture.NumberFormat);
            throw new ArgumentException(String.Format(CultureInfo.InvariantCulture.NumberFormat, "The token '{0}' is not a number at line {1} column {2}.",
                number, this.LineNumber, this.Column));
        }
        /// <summary>
        /// 若当前令牌为字符串类型，则返回该字符串。 
        /// </summary>
        public string GetStringValue() { return _currentToken; }

        /// <summary>
        /// 当前令牌类型。
        /// </summary>
        /// <returns></returns>
        public TokenType GetTokenType() { return _currentTokenType; }

        /// <summary>
        /// 读取下一个令牌，返回下一个令牌类型。
        /// </summary>
        /// <param name="ignoreWhitespace">是否忽略空白符</param>
        /// <returns>The TokenType of the next token.</returns>
        public TokenType NextToken(bool ignoreWhitespace)
        {
            TokenType nextTokenType;
            if (ignoreWhitespace)
            {
                nextTokenType = NextNonWhitespaceToken();
            }
            else
            {
                nextTokenType = NextTokenAny();
            }
            return nextTokenType;
        }

        /// <summary>
        ///  Returns the next token.
        /// </summary>
        /// <returns>The TokenType of the next token.</returns>
        public TokenType NextToken()
        {
            return NextToken(_ignoreWhitespace);
        }
        /// <summary>
        /// 读取下一个令牌，并返回其类型。
        /// </summary>
        /// <remarks>
        /// 实现方法：一个一个字符的读取，判断令牌是否一致，若不一致则停止读取。
        /// </remarks>
        /// <returns></returns>
        private TokenType NextTokenAny()
        {
            TokenType nextTokenType = TokenType.Eof;
            char[] chars = new char[1];
            _currentToken = "";
            _currentTokenType = TokenType.Eof;
            int finished = _reader.Read(chars, 0, 1);//判断是否继续读取，非 0 则读取。

            bool isNumber = false;
            bool isWord = false;
            byte[] ba = null;
#if SILVERLIGHT
			Encoding AE = System.Text.Encoding.Unicode;
#else
            ASCIIEncoding AE = new ASCIIEncoding();
#endif
            char[] ascii = null;
            Char currentCharacter;
            Char nextCharacter;
            while (finished != 0)
            {
                // funcKeyToDouble int to char
                ba = new Byte[] { (byte)_reader.Peek() };

                ascii = AE.GetChars(ba);

                currentCharacter = chars[0];
                nextCharacter = ascii[0];
                _currentTokenType = GetType(currentCharacter);
                nextTokenType = GetType(nextCharacter);

                // handling of words with _
                if (isWord && currentCharacter == '_')
                {
                    _currentTokenType = TokenType.Word;
                }
                // handing of words ending in numbers
                if (isWord && _currentTokenType == TokenType.Number)
                {
                    _currentTokenType = TokenType.Word;
                }

                if (_currentTokenType == TokenType.Word && nextCharacter == '_')
                {
                    //enable words with _ inbetween
                    nextTokenType = TokenType.Word;
                    isWord = true;
                }
                if (_currentTokenType == TokenType.Word && nextTokenType == TokenType.Number)
                {
                    //enable words ending with numbers
                    nextTokenType = TokenType.Word;
                    isWord = true;
                }

                // handle negative numbers
                if (currentCharacter == '-' && nextTokenType == TokenType.Number && isNumber == false)
                {
                    _currentTokenType = TokenType.Number;
                    nextTokenType = TokenType.Number;
                    //isNumber = true;
                }


                // this handles numbers with a decimal point
                if (isNumber && nextTokenType == TokenType.Number && currentCharacter == '.')
                {
                    _currentTokenType = TokenType.Number;
                }
                if (_currentTokenType == TokenType.Number && nextCharacter == '.' && isNumber == false)
                {
                    nextTokenType = TokenType.Number;
                    isNumber = true;
                }

                _colNumber++;
                if (_currentTokenType == TokenType.Eol)
                {
                    _lineNumber++;
                    _colNumber = 1;
                }

                _currentToken = _currentToken + currentCharacter;//令牌值

                //if (_currentTokenType==TokenType.Word && nextCharacter=='_')
                //{
                // enable words with _ inbetween
                //	finished = _reader.Read(chars,0,1);
                //}

                //判断当前令牌是否读取完毕，没有则继续循环读取。
                if (_currentTokenType != nextTokenType)
                {
                    finished = 0;
                }
                else if (_currentTokenType == TokenType.Symbol && currentCharacter != '-')
                {
                    finished = 0;
                }
                else
                {
                    finished = _reader.Read(chars, 0, 1);
                }
            }
            return _currentTokenType;
        }

        /// <summary>
        /// 通过输入字符，判断字符令牌类型（支持的令牌，此处包括字母、数字、空白、换行符，除此之外的认为是Symbol记号）。
        /// </summary>
        /// <param name="character">The character to determine.</param>
        /// <returns>The TokenType the character is.</returns>
        private static TokenType GetType(char character)
        {
            if (Char.IsDigit(character))
            {
                return TokenType.Number;
            }
            else if (Char.IsLetter(character))
            {
                return TokenType.Word;
            }
            else if (character == '\n')
            {
                return TokenType.Eol;
            }
            else if (Char.IsWhiteSpace(character) || Char.IsControl(character))
            {
                return TokenType.Whitespace;
            }
            else //(Char.IsSymbol(character))
            {
                return TokenType.Symbol;
            }

        }

        /// <summary>
        /// 读取下一个令牌，返回下一个令牌类型。忽略空白。
        /// </summary>
        /// <returns></returns>
        private TokenType NextNonWhitespaceToken()
        {

            TokenType tokentype = this.NextTokenAny();
            while (tokentype == TokenType.Whitespace || tokentype == TokenType.Eol)
            {
                tokentype = this.NextTokenAny();
            }

            return tokentype;
        }
        #endregion

    }

    /// <summary>
    /// 令牌（标记、记号）类型。在类StreamTokenizer中使用。
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// Indicates that the token is a word.
        /// </summary>
        Word,
        /// <summary>
        /// Indicates that the token is a number. 
        /// </summary>
        Number,
        /// <summary>
        ///End of line。  Indicates that the end of line has been read. The field can only have this value if the eolIsSignificant method has been called with the argument true. 
        /// </summary>
        Eol,
        /// <summary>
        ///End of Stream。  Indicates that the end of the input stream has been reached.
        /// </summary>
        Eof,
        /// <summary>
        /// 空白，含空字符、制表符、新行。Indictaes that the token is white space (space, tab, newline).
        /// </summary>
        Whitespace,
        /// <summary>
        /// 其它类型。 Characters that are not whitespace, numbers, etc...
        /// </summary>
        Symbol
    }    
}
