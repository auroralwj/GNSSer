//2014.05.26, czs, created 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Common;

namespace Geo.Referencing
{
        /// <summary>
        /// 首子午线。
        /// </summary>
        public class PrimeMeridian : IdentifiedObject
        { 
            /// <summary>
            /// 构造函数
            /// </summary>
            public  PrimeMeridian(){}
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="longitude"></param>
            /// <param name="angularUnit"></param>
            /// <param name="name"></param>
            public PrimeMeridian(double longitude, AngularUnit angularUnit, string name = null, string id = null, string abbrev=null)
                :base(  id ,name,   abbrev )
            {
                this.Longitude = longitude;
                this.AngularUnit = angularUnit;
                this.Name = name;
            }

            /// <summary>
            /// 角度单位
            /// </summary>
            public AngularUnit AngularUnit { get; set; }
            /// <summary>
            /// 子午线的经度
            /// </summary>
            public double Longitude { get; set; }

            #region 继承重写
            /// <summary>
            /// 继承重写
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return Longitude.GetHashCode();
            }


            /// <summary>
            /// Checks whether the values of this instance is equal to the values of another instance.
            /// Only parameters used for coordinate system are used for comparison.
            /// Name, abbreviation, authority, alias and remarks are ignored in the comparison.
            /// </summary>
            /// <param name="obj"></param>
            /// <returns>True if equal</returns>
            public override bool Equals(object obj)
            {
                if (!(obj is PrimeMeridian))
                    return false;
                PrimeMeridian prime = obj as PrimeMeridian;
                return prime.AngularUnit.Equals(this.AngularUnit) && prime.Longitude == this.Longitude;
            }

            #endregion
            
            #region Predefined prime meridans
            /// <summary>
            /// 格林尼治子午线。
            /// </summary>
            public static PrimeMeridian Greenwich
            {
                get { return new PrimeMeridian(){
                       Name="Greenwich"
          ,            Abbreviation = "Greenwich",
                       AngularUnit = AngularUnit.Degree,
                       Longitude = 0.0,
                       Id = "8901"
                }; }
            }
            /// <summary>
            /// Lisbon prime meridian
            /// </summary>
            public static PrimeMeridian Lisbon
            {
                get { return new PrimeMeridian(){
                       Name="Lisbon"
          ,            Abbreviation = "Lisbon",
                       AngularUnit = AngularUnit.Degree,
                       Longitude = -9.0754862,
                       Id = "8902"
                }; }
            }
             
            /// <summary>
            /// Paris prime meridian.
            /// Value adopted by IGN (Paris) in 1936. Equivalent to 2 deg 20min 14.025sec. Preferred by EPSG to earlier value of 2deg 20min 13.95sec (2.596898 grads) used by RGS London.
            /// </summary>
            public static PrimeMeridian Paris
            {
                get { return new PrimeMeridian(){
                       Name="Paris"
          ,            Abbreviation = "Paris",
                       AngularUnit = AngularUnit.Degree,
                       Longitude = 2.5969213,
                       Id = "8903"
                };  
             }
            }
            /// <summary>
            /// Bogota prime meridian
            /// </summary>
            public static PrimeMeridian Bogota {
                get { return new PrimeMeridian(){
                       Name="Bogota"
          ,            Abbreviation = "Bogota",
                       AngularUnit = AngularUnit.Degree,
                       Longitude = -74.04513,
                       Id = "8904"
                };  
             }
            }
             
            /// <summary>
            /// Madrid prime meridian
            /// </summary>
            public static PrimeMeridian Madrid {
                get { return new PrimeMeridian(){
                       Name="Madrid"
          ,            Abbreviation = "Madrid",
                       AngularUnit = AngularUnit.Degree,
                       Longitude = -3.411658,
                       Id = "8905"
                };  
             }
            }
             
            /// <summary>
            /// Rome prime meridian
            /// </summary>
            public static PrimeMeridian Rome{
                get { return new PrimeMeridian(){
                       Name="Rome"
          ,            Abbreviation = "Rome",
                       AngularUnit = AngularUnit.Degree,
                       Longitude = 12.27084,
                       Id = "8906"
                };  
             }            
        }
            /// <summary>
            /// Bern prime meridian.
            /// 1895 value. Newer value of 7 deg 26 min 22.335 sec E determined in 1938.
            /// </summary>
            public static PrimeMeridian Bern{
                get { return new PrimeMeridian(){
                       Name="Bern"
          ,            Abbreviation = "Bern",
                       AngularUnit = AngularUnit.Degree,
                       Longitude = 7.26225,
                       Id = "8907"
                };  
             }            
        } 
            /// <summary>
            /// Jakarta prime meridian
            /// </summary>
            public static PrimeMeridian Jakarta{
                get { return new PrimeMeridian(){
                       Name="Jakarta"
          ,            Abbreviation = "Jakarta",
                       AngularUnit = AngularUnit.Degree,
                       Longitude = 106.482779,
                       Id = "8908"
                };  
             }            
        } 
             
            /// <summary>
            /// Ferro prime meridian.
            /// Used in Austria and former Czechoslovakia.
            /// </summary>
            public static PrimeMeridian Ferro{
                get { return new PrimeMeridian(){
                       Name="Ferro"
          ,            Abbreviation = "Ferro",
                       AngularUnit = AngularUnit.Degree,
                       Longitude = -17.4,
                       Id = "8909"
                };  
             }            
        }  
            /// <summary>
            /// Brussels prime meridian
            /// </summary>
            public static PrimeMeridian Brussels{
                get { return new PrimeMeridian(){
                       Name="Brussels"
          ,            Abbreviation = "Brussels",
                       AngularUnit = AngularUnit.Degree,
                       Longitude = 4.220471,
                       Id = "8910"
                };  
             }            
        }  
             
            /// <summary>
            /// Stockholm prime meridian
            /// </summary>
            public static PrimeMeridian Stockholm{
                get { return new PrimeMeridian(){
                       Name="Stockholm"
          ,            Abbreviation = "Stockholm",
                       AngularUnit = AngularUnit.Degree,
                       Longitude = 18.03298,
                       Id = "8911"
                };  
             }            
        }   
            /// <summary>
            /// Athens prime meridian.
            /// Used in Greece for older mapping based on Hatt projection.
            /// </summary>
            public static PrimeMeridian Athens{
                get { return new PrimeMeridian(){
                       Name="Athens"
          ,            Abbreviation = "Athens",
                       AngularUnit = AngularUnit.Degree,
                       Longitude = 23.4258815,
                       Id = "8912"
                };  
             }            
        }    
            /// <summary>
            /// Oslo prime meridian.
            /// Formerly known as Kristiania or Christiania.
            /// </summary>
            public static PrimeMeridian Oslo{
                get { return new PrimeMeridian(){
                       Name="Oslo"
          ,            Abbreviation = "Oslo",
                       AngularUnit = AngularUnit.Degree,
                       Longitude = 10.43225,
                       Id = "8913"
                };  
             }            
        }    
            #endregion

        }












}
