using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gnsser.Data.Sinex
{

    public class ParameterType
    {
        public const string STAX = "STAX";// station X coordinate, m	
        public const string STAY = "STAY";// station Y coordinate, m	
        public const string STAZ = "STAZ";// station Z coordinate, m	
        public const string VELX = "VELX";// station X velocity, m/y	
        public const string VELY = "VELY";// station Y velocity, m/y	
        public const string VELZ = "VELZ";// station Z velocity, m/y	
        public const string XGC = "XGC";// geocenter X coordinate, m	
        public const string YGC = "YGC";// geocenter Y coordinate, m	
        public const string ZGC = "ZGC";// geocenter Z coordinate, m	
        public const string RS_RA = "RS_RA";// radio source right ascension, rad	"		
        public const string RS_DE = "RS_DE";// radio source declin., rad	
        public const string RS_RAR = "RS_RAR";// radio source right 	ascension rate, rad/y	"		
        public const string RS_DER = "RS_DER";// radio source declination rate, rad/y	"		
        public const string RS_PL = "RS_PL";// radio source parallax, rad	
        public const string LOD = "LOD";// length of secondOfWeek, ms	
        public const string UT = "UT";// delta time UT1	;//UTC, ms
        public const string XPO = "XPO";// X polar motion, mas	
        public const string YPO = "YPO";// Y polar motion, mas	
        public const string XPOR = "XPOR";// X polar motion rate, mas/d	
        public const string YPOR = "YPOR";// Y polar motion rate, mas/d	
        public const string NUT_LN = "NUT_LN";// – nutation correction 	in longitude, mas	"		
        public const string NUT_OB = "NUT_OB";//– nutation correction 	in obliquity, mas	"		
        public const string NUTRLN = "NUTRLN";// nutation rate in longitude, mas/d	"		
        public const string NUTROB = "NUTROB";// nutation rate in 	obliquity, mas/d	"		
        public const string NUT_X = "NUT_X";// nutation correction X, mas	
        public const string NUT_Y = "NUT_Y";// nutation correction Y, mas	
        public const string NUTR_X = "NUTR_X";// – nutation rate in X mas/d	"		
        public const string NUTR_Y = "NUTR_Y";// – nutation rate in Y mas/d	"		
        public const string SAT__X = "SAT__X";// Satellite X coord., m	
        public const string SAT__Y = "SAT__Y";// Satellite Y coord., m	
        public const string SAT__Z = "SAT__Z";// Satellite Z coord., m	
        public const string SAT_VX = "SAT_VX";// Satellite X velocity, m/s	
        public const string SAT_VY = "SAT_VY";// Satellite Y velocity, m/s	
        public const string SAT_VZ = "SAT_VZ";// Satellite Z velocity, m/s	
        public const string SAT_RP = "SAT_RP";// Radiation pressure, 	
        public const string SAT_GX = "SAT_GX";// GX scale, 	
        public const string SAT_GZ = "SAT_GZ";// GZ scale, 	
        public const string SATYBI = "SATYBI";// GY bias, m/nStr	
        public const string TROTOT = "TROTOT";// wet + dry Trop. delay, m	
        public const string TRODRY = "TRODRY";// dry Trop. delay, m	
        public const string TROWET = "TROWET";// wet Trop. delay, m	
        public const string TGNTOT = "TGNTOT";//north north (wet + dry), m	"		
        public const string TGNWET = "TGNWET";// – troposphere gradient in north (only wet), m	"		
        public const string TGNDRY = "TGNDRY";// – troposphere gradient 	in north (only dry), m	"		
        public const string TGETOT = "TGETOT";// troposphere gradient 	in east (wet + dry), m	"		
        public const string TGEWET = "TGEWET";// in east (only wet), m	"		
        public const string TGEDRY = "TGEDRY";// – troposphere gradient 	in east (only dry), m "
        public const string RBIAS = "RBIAS";// range bias, m	
        public const string TBIAS = "TBIAS";// time bias, ms	
        public const string SBIAS = "SBIAS";// scale bias, ppb	
        public const string ZBIAS = "ZBIAS";// troposphere bias at zenith, m	"		
        public const string AXI_OF = "AXI_OF";// – VLBI antenna axis 	offset, m	
        public const string SATA_Z = "SATA_Z";// – sat. antenna Z offset m	"		
        public const string SATA_X = "SATA_X";// – sat. antenna X offset, m	"		
        public const string SATA_Y = "SATA_Y";// – sat. antenna Y offset, m	"		
        public const string CN = "CN";// spherical harmonic 	coefficient C_nm 
        public const string SN = "SN";// spherical harmonic coefficient S_nm 	

    }


}
