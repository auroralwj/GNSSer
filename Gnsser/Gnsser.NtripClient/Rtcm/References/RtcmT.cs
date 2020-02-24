using System;
using System.Collections.Generic;
using System.Text;

namespace Gnsser.Ntrip
{ 

	/// <summary>
	/// 解码程序
	/// </summary>
	public class RtcmT 
	{
		internal int local = 0;

        internal int debug;

        internal int this_word, data_word;
        internal char next_bits;

        internal int rtcm_state, fail_count, p_fail, preamble_flag;
        internal int sync_bit, fill_shift, word_sync, frame_sync, frame_fill;
        internal int word_count, frame_count, pf_count;
        internal int msg_type, station_id, z_count, seqno, msg_len, health;
        internal char[] parity_of = { };
        internal int[] fillptr;
        internal int[] pfptr;

        protected internal int[] message { get; set; }

        internal IList<string> satelliteList { get; set; } 



		/* message types */

		internal readonly int MSG_FULLCOR = 1;
		internal readonly int MSG_REFPARM = 3;
		internal readonly int MSG_DATUM = 4;
		internal readonly int MSG_CONHLTH = 5;
		internal readonly int MSG_NULL = 6;
		internal readonly int MSG_BEACALM = 7;
		internal readonly int MSG_SUBSCOR = 9;
		internal readonly int MSG_SPECIAL = 16;

		internal const int P_30MASK = 0x40000000;
		internal const int P_31MASK = unchecked((int)0x80000000);

		internal const int PARITY_25 = unchecked((int)0xbb1f3480);
		internal const int PARITY_26 = 0x5d8f9a40;
		internal const int PARITY_27 = unchecked((int)0xaec7cd00);
		internal const int PARITY_28 = 0x5763e680;
		internal const int PARITY_29 = 0x6bb1f340;
		internal const int PARITY_30 = unchecked((int)0x8b7a89c0);

		internal const int W_DATA_MASK = 0x3fffffc0;

		internal const int PREAMBLE = 0x19800000;
		internal const int PREAMBLE_MASK = 0x3fc00000;
		internal int DATA_SHIFT = 6;
		internal int NO_SYNC = 0;
		internal readonly int WORD_SYNCING = 1;
		internal readonly int WORD_SYNC = 2;
		internal readonly int FRAME_SYNCING = 3;
		internal readonly int FULL_SYNC = 4;

		internal int W_SYNC_TEST = 6;
		internal int F_SYNC_TEST = 2; //原值为3，会跳过一个包头
		internal int P_FAIL_TEST = 10;

		internal int FILL_BASE = 24;
		internal char[] reverse_bits = {};
		/* field scale factors */

		internal double ZCOUNT_SCALE = 0.6; // sec
		internal double RANGE_SMALL = 0.02; // metres
		internal double RANGE_LARGE = 0.32; // metres
		internal double RANGERATE_SMALL = 0.002; // metres/sec
		internal double RANGERATE_LARGE = 0.032; // metres/sec
		internal double XYZ_SCALE = 0.01; // metres
		internal double DXYZ_SCALE = 0.1; // metres
		internal double LA_SCALE = 90.0 / 32767.0; // degrees
		internal double LO_SCALE = 180.0 / 32767.0; // degrees
		internal double FREQ_SCALE = 0.1; // kHz
		internal double FREQ_OFFSET = 190.0; // kHz
		internal double CNR_OFFSET = 24; // dB
		internal double TU_SCALE = 5; // minutes

		internal virtual int preamble()
		{
			bool t = ((data_word & PREAMBLE_MASK) == PREAMBLE);
			return t == true ? 1 : 0;
		}

        public static void Main0(string[] args)
        {
            RtcmT rt = new RtcmT();
            //		char b;
            rt.message = new int[60];
            char[] buf = new char[1024];
            int size = 0;
            System.IO.StreamReader read = new System.IO.StreamReader("11.dat"); //创建文件读取类对象
            //System.IO.StreamReader read = new System.IO.StreamReader(fileR,  Encoding.ASCII); //让文件读取作为缓冲读取的构造参数
            while ((size = read.Read(buf, 0, 1024)) != -1)
            {
                for (int i = 0; i < size; i++)
                {
                    rt.new_byte(buf[i]);
                }
            }
            //		String aa = "fAB@k^r`dV@ChUE`sNXE@JFDf|ApRG@`Pvo@TmN`aGlxqKChc@@H_CAYtw|lIPhCJB_M}V|c^EX`E|w_MhC`QSPX@^@@`@D]Rc_Zc@b|yCb~`XMq}YF|KaA`[]@Cy{b`QuwUPXO@XjBCBJr|kjTpbPQ^i";
            //		char[] temp = aa.toCharArray();
            //		// TODO Auto-generated method stub
            //		int i = 0;
            //		for (int j = 0; j < temp.length; j++) {
            //			rt.new_byte(temp[i]);
            //		}
            //		while ((b = temp[i]) != -1) {
            //			
            //			i++;
            //		}
        }

		public virtual void new_byte(char b)
		{

			switch ((int)((uint)b >> DATA_SHIFT))
			{

			case 0:
			case 2:
				// if 0
				// #ifdef DEBUG
				// fprintf(stderr, "unknown byte type %d (%d 0x%0x)\n", b >>
				// DATA_SHIFT,
				// b, b);
				// #endif
				// #endif
				return;

			case 3: // status
				status_byte(b);
				return;

			case 1: // satData
				data_byte(b);
				return;
			}
		
        }

        public enum WordMarker
        {
            WORD_SYNCING,
            WORD_SYNC,
            FULL_SYNC,
            FRAME_SYNCING
        }

		internal virtual void status_byte(char b)
		{
			Console.WriteLine(b);
	//		#if 0
	//		#if defined(PRINTSTATUS)
	//		    printf("-\tstatus\t-\t%d 0x%x\n", b, b);
	//		#endif
	//		#endif
		}

		/* take a satData byte and process it. This will change state according to
		 * the satData, place parity-checked satData in a buffer and call a function to
		 * process completed frames.
		 */
		internal virtual void data_byte(char b)
		{

			b = reverse_bits[b & 0x3f]; //b & 0x3f取低位，匹配2*6=64位字符，找到低6为的字符

			if (rtcm_state == NO_SYNC)
			{
				if (find_sync(b) != 0)
				{
					state_change(WORD_SYNCING);
					//System.out.println("sync_bit = "+sync_bit);
					// #if 0
					// printf("M\tsync_bit: %d\n", sync_bit);
					// #endif
					word_sync = 1;
					next_word();
				}
			}
			else if (filled_word(b) != 0)
			{
                switch ((WordMarker)rtcm_state)
				{
                    case WordMarker.WORD_SYNCING:
					data_word = parity_ok();
					next_word();
					if (data_word != 0)
					{
						if (++word_sync >= W_SYNC_TEST)
						{
							state_change(WORD_SYNC);
							p_fail = 0;
							frame_sync = 1;
							if (preamble() != 0)
							{
							/*
													 * just in case we hit one
													 * immediately
													 */
								//System.out.println("\n--------------1");
															frame_start();
								state_change(FRAME_SYNCING);
							}
						}
					}
					else
					{
						if (--word_sync <= 0)
						{
							state_change(NO_SYNC);
							return;
						}
					}
					break;

                    case WordMarker.WORD_SYNC: // look for frame start
					find_start();
					break;

                    case WordMarker.FRAME_SYNCING:
					fill_frame();
					break;

                    case WordMarker.FULL_SYNC:
					fill_frame();
					break;
				}
			}
		}

		internal virtual void _change(int s)
		{
			// #if 0
			// printf("M\tstate change: %s -> %s\n",
			// state_name[rtcm_state], state_name[s]);
			// fflush(stdout);
			// #endif
			rtcm_state = s;
		}

		internal virtual int find_sync(char b)
		{
			int i;

			b <<= 2; //左移两位，变成纯低6为运算
			i = 1;

			while (i <= DATA_SHIFT)
			{
				this_word <<= 1; //this_word初始化为0
				 //取b字符最高位，如果是非0就为1，否则为0 this_word与上1：或   有1为1， 全0为0。
				//，所以这个操作完还是1或者0，判断b的最高位是1还是0
				this_word |= ((b & 0x80) != 0) ? 1 : 0; // next bit into 32 bits
				b <<= 1; //最高位外移
				data_word = parity_ok();
				if ((data_word != 0))
				{
					sync_bit = (i == DATA_SHIFT) ? 0 : i;
					fill_shift = FILL_BASE - DATA_SHIFT + i;
					next_bits = (char)((int)((uint)b >> 2));
					return 1;
				}
				i++;
			}
			return 0;
		}

		internal virtual void next_word()
		{
			this_word = (this_word << 30) | (next_bits << 24);
		}
		/* check the parity on this_word. bits 31,30 are parity on previous word.
		 * bits 29-6 are satData bits. Bits 5-0 are parity bits.
		 */
		internal virtual int parity_ok()
		{
			int t, th, p;

			th = this_word;
			if ((th & P_30MASK) != 0)
			{
				th ^= W_DATA_MASK;
			}

			t = th & PARITY_25;
			p = parity_of[t & 0xff] ^ parity_of[((int)((uint)t >> 8)) & 0xff] ^ parity_of[((int)((uint)t >> 16)) & 0xff] ^ parity_of[((int)((uint)t >> 24))];
			t = th & PARITY_26;
			p = (p << 1) | (parity_of[t & 0xff] ^ parity_of[((int)((uint)t >> 8)) & 0xff] ^ parity_of[((int)((uint)t >> 16)) & 0xff] ^ parity_of[((int)((uint)t >> 24))]);
			t = th & PARITY_27;
			p = (p << 1) | (parity_of[t & 0xff] ^ parity_of[((int)((uint)t >> 8)) & 0xff] ^ parity_of[((int)((uint)t >> 16)) & 0xff] ^ parity_of[((int)((uint)t >> 24))]);
			t = th & PARITY_28;
			p = (p << 1) | (parity_of[t & 0xff] ^ parity_of[((int)((uint)t >> 8)) & 0xff] ^ parity_of[((int)((uint)t >> 16)) & 0xff] ^ parity_of[((int)((uint)t >> 24))]);
			t = th & PARITY_29;
			p = (p << 1) | (parity_of[t & 0xff] ^ parity_of[((int)((uint)t >> 8)) & 0xff] ^ parity_of[((int)((uint)t >> 16)) & 0xff] ^ parity_of[((int)((uint)t >> 24))]);
			t = th & PARITY_30;
			p = (p << 1) | (parity_of[t & 0xff] ^ parity_of[((int)((uint)t >> 8)) & 0xff] ^ parity_of[((int)((uint)t >> 16)) & 0xff] ^ parity_of[((int)((uint)t >> 24))]);

			if (this_word == 0 || ((this_word & 0x3f) != p))
			{ // if (!this_word
				if (rtcm_state > WORD_SYNCING)
				{
					pf_count++;
				}
				return 0;
			}

			return th;
		}

		internal virtual void state_change(int s)
		{
			// #if 0
			// printf("M\tstate change: %s -> %s\n",
			// state_name[rtcm_state], state_name[s]);
			// fflush(stdout);
			// #endif
			rtcm_state = s;
		}

		internal virtual int filled_word(char b)
		{

			if (fill_shift <= 0)
			{ // can complete fill
				if (fill_shift != 0)
				{
					next_bits = (char)(b << (DATA_SHIFT + fill_shift));
					this_word |= (int)((uint)b >> (-fill_shift));
				}
				else
				{
					next_bits = (char)0;
					this_word |= b;
				}
				fill_shift += FILL_BASE;
				word_count++;
				return 1;
			}
			this_word |= b << fill_shift;
			fill_shift -= DATA_SHIFT;
			return 0;
		}

		internal virtual void frame_start()
		{
			frame_fill = -1;
			fillptr = message;
			pfptr = null;
					local = 0;
					//System.out.println("frame_start():"+data_word);
			buffer(data_word);
		}

		internal virtual void buffer(int w)
		{
					//System.out.println("\n" + local+"-----------local");
			try
			{
				fillptr[local] = w;
				local++;
			}
			catch (Exception e)
			{
				// TODO: handle exception
				Console.WriteLine(e.ToString());
				Console.Write(e.StackTrace);
			}
	//		System.out.println(w+"-----------w");
		}

		internal virtual void find_start()
		{
			data_word = parity_ok();
			if (data_word != 0)
			{
				p_fail = 0;
			}
			else if (++p_fail >= P_FAIL_TEST)
			{ // too many consecutive parity fails
				state_change(NO_SYNC);
				return;
			}
					//System.out.println("find_start():"+data_word);
			next_word();
			if (preamble() != 0)
			{
				seqno = -1; // resync at next word
							//System.out.println("\n--------------2");
				frame_start();
				state_change(FRAME_SYNCING);
			}
		}

		internal virtual void fill_frame()
		{
			int seq;
			data_word = parity_ok();
			if (data_word != 0)
			{
				p_fail = 0;
			}
			else if (++p_fail >= P_FAIL_TEST)
			{
				state_change(NO_SYNC);
				return;
			}
			next_word();
			frame_fill++; // another word
			if (frame_fill == 0)
			{ // this is second header word
				if (data_word == 0)
				{ // parity fail - bad news!
					state_change(WORD_SYNC); // lost frame sync
					frame_sync = F_SYNC_TEST - 1; // resync rapidly
					return;
				}
							//System.out.println("\ndata_word = "+data_word + ",local = " + local);
				buffer(data_word);
				seq = ((int)((uint)data_word >> 14)) & 0x7;
				msg_len = ((int)((uint)data_word >> 9)) & 0x1f;
							//System.out.println("\nmsg_len = " + msg_len);
				// #if 0
				// #ifdef DEBUG
				// if (debug)
				//System.out.println( "ff=%d: %08x %08x %d %d %d %d %s\n"+ frame_fill+","+
						//","+fillptr+","+
				// data_word+","+ msg_len+","+ word_count+","+ pf_count+","+ seqno+",");
				// #endif
				// #endif
				if ((seqno < 0) || (((seqno + 1) & 0x7) == seq))
				{
					seqno = seq; // resync
				}
				else
				{ // sequence error
					state_change(WORD_SYNC); // to be on the safe side
					// #if 0
					// fprintf(stderr,"2\n");
					// #endif
					return;
				}
			}
			else if (frame_fill > msg_len)
			{ // should be next preamble
				// #if 0
				// #ifdef DEBUG
				// if (debug)
				//System.out.println("ff=%d: %08x %08x %d %d %d %d %s\n"+frame_fill);
				// (u_int)fillptr,
				// data_word, msg_len, word_count, pf_count, seqno,
				// state_name[rtcm_state]);
				// #endif
				// #endif
				if (rtcm_state == FRAME_SYNCING)
				{ // be very tough
					// boolean t = (data_word!=0)&&preamble()!=0;
					if (((data_word != 0) && (preamble() != 0)) == false)
					{
						state_change(WORD_SYNC); // start again
					}
					else if (++frame_sync >= F_SYNC_TEST)
					{
						/* frame_count = 0; */
						state_change(FULL_SYNC);
						new_frame(); /*
									 * output the last frame acquired before we
									 * start a new one
									 */
					}
									//System.out.println("\n--------------3");
					frame_start(); // new frame here
				}
				else
				{
									//System.out.println("\n--------------4");
					frame_start(); // new frame here
					if (data_word == 0) /*
										 * parity error on preamble - keep sync but
										 * lose message
										 */
					{
						pfptr = message; // indicates dud message
					}
					else if (preamble() == 0)
					{ // good word but no preamble!
						state_change(WORD_SYNC); // can't carry on
					}
				}
			}
			else
			{ // other message words
				// #if 0
				// #ifdef DEBUG
				// if (debug)
				//System.out.println( "ff=%d: %08x %08x %d %d %d %d %s\n"+ frame_fill);
				// (u_int)fillptr,
				// data_word, msg_len, word_count, pf_count, seqno,
				// state_name[rtcm_state]);
				// #endif
				// #endif
				if (data_word == 0 && pfptr == null)
				{
					pfptr = fillptr; // mark the (prevObj) error
				}
				buffer(data_word);
				if ((frame_fill == msg_len) && (rtcm_state == FULL_SYNC))

				{
							/*
																		 * frame
																		 * completed
																		 */
					new_frame();
				}
			}
		}

        public enum MSGType
        {
            MSG_FULLCOR,
            MSG_SUBSCOR
        }

		internal virtual void new_frame()
		{
			// char s[];

			frame_count++;
			if (pfptr == message) // dud frame
			{
				return;
			}

			msg_type = ((int)((uint)message[0] >> 16)) & 0x3f;
			station_id = ((int)((uint)message[0] >> 6)) & 0x3ff;
			z_count = ((int)((uint)message[1] >> 17)) & 0x1fff;
			health = ((int)((uint)message[1] >> 6)) & 0x7;
			// #if 0
			// if (pfptr)
			// sprintf(s, "\tT\t%d", (pfptr-message)-2);
					//for(int i=0;i<message.length;i++)
						//System.out.print(message[i] + ",");
			// (z_count*ZCOUNT_SCALE), seqno, msg_len, health,
			// (pfptr)?s:"");
			// #endif
            switch ((MSGType)msg_type)
			{

                case MSGType.MSG_FULLCOR:
                case MSGType.MSG_SUBSCOR:
				printcor();
				break;

	//		case MSG_REFPARM:
	//			printref();
	//			break;
	//
	//		case MSG_DATUM:
	//			printdatum();
	//			break;
	//
	//		case MSG_CONHLTH:
	//			printconh();
	//			break;
	//
	//		case MSG_NULL:
	//			printnull();
	//			break;
	//
	//		case MSG_BEACALM:
	//			printba();
	//			break;
	//
	//		case MSG_SPECIAL:
	//			printspec();
	//			break;
	//
	//		case 18: /* RTK Uncorrected Carrier Phases */
	//			// msgRTKUncorrectedCarrierPhases();
	//			break;
	//
	//		case 19: /* RTK Uncorrected Pseudoranges */
	//			// msgRTKUncorrectedPseudoranges();
	//			break;

			default:
				// #if 0
				Console.WriteLine("msg_type Error ! \n");
				// #endif
				break;
			}
		}

		internal virtual void printcor()
		{
			satelliteList = new List<string>();
			int i, w;
			int m, n;
			int scale, udre, sat, range, rangerate, iod;

			i = 0;
			w = 2;
			m = 0;

					Console.WriteLine("msg_type = " + msg_type + ",station_id = " + station_id + ",z_count = " + z_count * ZCOUNT_SCALE + ",seqno = " + seqno + ",msg_len = " + msg_len + ",health = " + health);

			if (pfptr != null)
			{
				msg_len = local - 2; //            msg_len = (pfptr - message) - 2;

				n = msg_len % 5;
				if (n == 1 || n == 3)
				{
					msg_len--;
				}
				if (msg_len < 2)
				{
					return;
				}
			}
					//System.out.println("msg_len = " + msg_len);
			while (w < msg_len + 2)
			{
				if ((i & 0x3) == 0)
				{
					m = message[w++] & W_DATA_MASK;
					scale = (int)((uint)m >> 29) & 0x1;
					udre = ((int)((uint)m >> 27)) & 0x3;
					sat = ((int)((uint)m >> 22)) & 0x1f;
					range = ((int)((uint)m >> 6)) & 0xffff;
					if (range > 32767)
					{
						range -= 65536;
					}
					m = message[w++] & W_DATA_MASK;
					rangerate = ((int)((uint)m >> 22)) & 0xff;
					if (rangerate > 127)
					{
						rangerate -= 256;
					}
					iod = ((int)((uint)m >> 14)) & 0xff;
					i++;
				}
				else if ((i & 0x3) == 1)
				{
					scale = (int)((uint)m >> 13) & 0x1;
					udre = ((int)((uint)m >> 11)) & 0x3;
					sat = ((int)((uint)m >> 6)) & 0x1f;
					m = message[w++] & W_DATA_MASK;
					range = ((int)((uint)m >> 14)) & 0xffff;
					if (range > 32767)
					{
						range -= 65536;
					}
					rangerate = ((int)((uint)m >> 6)) & 0xff;
					if (rangerate > 127)
					{
						rangerate -= 256;
					}
					m = message[w++] & W_DATA_MASK;
					iod = ((int)((uint)m >> 22)) & 0xff;
					i++;
				}
				else
				{
					scale = (int)((uint)m >> 21) & 0x1;
					udre = ((int)((uint)m >> 19)) & 0x3;
					sat = ((int)((uint)m >> 14)) & 0x1f;
					range = (m << 2) & 0xff00;
					m = message[w++] & W_DATA_MASK;
					range |= ((int)((uint)m >> 22)) & 0xff;
					if (range > 32767)
					{
						range -= 65536;
					}
					rangerate = ((int)((uint)m >> 14)) & 0xff;
					if (rangerate > 127)
					{
						rangerate -= 256;
					}
					iod = ((int)((uint)m >> 6)) & 0xff;
					i += 2;
				}
							//System.out.println(message[w]);

				Console.WriteLine("oFileName = " + sat + ",udre = " + udre + ",iod = " + iod + ",range = " + range * ((scale != 0)?RANGE_LARGE:RANGE_SMALL) + ",rangerate = " + rangerate * ((scale != 0)?RANGERATE_LARGE:RANGERATE_SMALL));
				var satellite = sat+ (range * ((scale != 0)?RANGE_LARGE:RANGE_SMALL)) + "" + (rangerate * ((scale != 0)?RANGERATE_LARGE:RANGERATE_SMALL));
				satelliteList.Add(satellite);
			}
					Console.WriteLine("");
		}

		internal virtual void printref()
		{
			int x, y, z;

			if (pfptr != null)
			{
				return;
			}
			x = ((message[2] & W_DATA_MASK) << 2) | ((int)((uint)(message[3] & W_DATA_MASK) >> 22));
			y = ((message[3] & W_DATA_MASK) << 10) | ((int)((uint)(message[4] & W_DATA_MASK) >> 14));
			z = ((message[4] & W_DATA_MASK) << 18) | ((int)((uint)(message[5] & W_DATA_MASK) >> 6));

			// onPosition(x*XYZ_SCALE, y*XYZ_SCALE, z*XYZ_SCALE);

			// #if 0
			Console.WriteLine("R\t%.2f\t%.2f\t%.2f\n" + x * XYZ_SCALE + "," + y * XYZ_SCALE + "," + z * XYZ_SCALE);
			// #endif
		}

		/*
		 * printba - print beacon almanac
		 */

		internal virtual void printba()
		{
			int la, lo, range, freq, hlth, id, bitrate;

			if (pfptr != null)
			{
				return;
			}
			la = (((int)((uint)message[2] >> 14)) & 0xffff);
			if (la > 32767)
			{
				la -= 65536;
			}
			lo = ((message[2] << 2) & 0xff00) | (((int)((uint)message[3] >> 22)) & 0xff);
			if (lo > 32767)
			{
				lo -= 65536;
			}
			range = (((int)((uint)message[3] >> 12)) & 0x3ff);
			freq = ((message[3]) & 0xfc0) | (((int)((uint)message[4] >> 24)) & 0x3f);
			hlth = (((int)((uint)message[4] >> 22)) & 0x3);
			id = (((int)((uint)message[4] >> 12)) & 0x3ff);
			bitrate = (((int)((uint)message[4] >> 9)) & 0x7);

			// #if 0
						//printf("A\t%.4f\t%.4f\t%d\t%.1f\t%d\t%d\t%d\n", (la*LA_SCALE), (lo*LO_SCALE),
								//range, (freq*FREQ_SCALE+FREQ_OFFSET), hlth, id, tx_speed[bitrate]);
			// #endif
		}

		/*
		 * printspec - print text of special message
		 */

		internal virtual void printspec()
		{
			int i, d, c;
			// #if 0
			// if (pfptr)
			// msg_len = (pfptr-message)>>2;
			// printf("T\t");
			// for (i=0; i< msg_len; i++) {
			// d = message[i+2] & W_DATA_MASK;
			// if ((c=d>>22)) putchar(c); else break;
			// if ((c=((d>>14) & 0xff))) putchar(c); else break;
			// if ((c=((d>>6) & 0xff))) putchar(c); else break;
			// }
			// printf("\n");
			// #endif
		}

		/*
		 * printnull - print a marker for a null message
		 */

		internal virtual void printnull()
		{
			// #if 0
			// printf("N\n");
			// #endif
		}

		/*
		 * printdatum - print datum message
		 */

		internal virtual void printdatum()
		{
			char[] dname = new char[6];
			char[] dn;
			int d, dgnss, dat;
			int dx, dy, dz;

			if (pfptr != null)
			{
				return;
			}
			d = message[2] & W_DATA_MASK;
			dgnss = (int)((uint)d >> 27);
			dat = ((int)((uint)d >> 26)) & 1;
			dname[0] = (char)(((int)((uint)d >> 14)) & 0xff);
			if (dname[0] != '\0')
			{ // not null
				dname[1] = (char)(((int)((uint)d >> 6)) & 0xff);
				d = message[3] & W_DATA_MASK;
				dname[2] = (char)(((int)((uint)d >> 22)) & 0xff);
				dname[3] = (char)(((int)((uint)d >> 14)) & 0xff);
				dname[4] = (char)(((int)((uint)d >> 6)) & 0xff);
				dname[5] = '\0';
				dn = dname;
			}
			else
			{
				dn = null;
			}
			// #if 0
			Console.WriteLine("D\t%s\t%1d\t%s" + ((dgnss == 0)?"GPS":((dgnss == 1)?"GLONASS":"???")) + "," + dat + "," + dn);
			// #endif
			if (msg_len > 2)
			{
				d = message[4] & W_DATA_MASK;
				dx = ((int)((uint)d >> 14)) & 0xffff;
				if (dx > 32767)
				{
					dx -= 65536;
				}
				dy = (d << 2) & 0xff00;
				d = message[5] & W_DATA_MASK;
				dy |= ((int)((uint)d >> 22)) & 0xff;
				if (dy > 32767)
				{
					dy -= 65536;
				}
				dz = ((int)((uint)d >> 6)) & 0xffff;
				if (dz > 32767)
				{
					dz -= 65536;
				}
				// #if 0
				Console.WriteLine("\t%.1f\t%.1f\t%.1f" + "," + dx * DXYZ_SCALE + "," + dy * DXYZ_SCALE + "," + dz * DXYZ_SCALE);
				// #endif
				// }
				// #if 0
				// printf("\n");
				// #endif
			}
		}

		/*
		 * printconh - print constellation health message
		 */

		internal virtual void printconh()
		{
			int i, d, sat, iodl, hlth, cnr, he, nd, lw, tu;

			if (pfptr != null)
			{
				msg_len = local - 2; //            msg_len = (pfptr - message) - 2;
				if (msg_len == 0)
				{
					return;
				}
			}
			for (i = 0; i < msg_len; i++)
			{
				d = message[i + 2] & W_DATA_MASK;
				sat = ((int)((uint)d >> 24)) & 0x1f;
				if (sat == 0)
				{
					sat = 32;
				}
				iodl = ((int)((uint)d >> 23)) & 1;
				hlth = ((int)((uint)d >> 20)) & 0x7;
				cnr = ((int)((uint)d >> 15)) & 0x1f;
				he = ((int)((uint)d >> 14)) & 1;
				nd = ((int)((uint)d >> 13)) & 1;
				lw = ((int)((uint)d >> 12)) & 1;
				tu = ((int)((uint)d >> 8)) & 0x0f;
				// #if 0
				Console.WriteLine("C\t%2d\t%1d  %1d\t%2d\t%1d  %1d  %1d\t%2d\n" + "," + sat + "," + iodl + "," + hlth + "," + (cnr != 0?(cnr + CNR_OFFSET):-1) + "," + he + "," + nd + "," + lw + "," + tu * TU_SCALE);
				// #endif
			}
		}
	}

}