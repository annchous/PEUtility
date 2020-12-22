using System;

namespace PEUtility.Headers
{
    public struct DosHeader
    {
        public UInt16 e_magic;
        public UInt16 e_cblp;
        public UInt16 e_cp;
        public UInt16 e_crlc;
        public UInt16 e_cparhdr;
        public UInt16 e_minalloc;
        public UInt16 e_maxalloc;
        public UInt16 e_ss;
        public UInt16 e_sp;
        public UInt16 e_csum;
        public UInt16 e_ip;
        public UInt16 e_cs;
        public UInt16 e_ifarlc;
        public UInt16 e_ovno;
        public UInt16 e_res0;
        public UInt16 e_res1;
        public UInt16 e_res2;
        public UInt16 e_res3;
        public UInt16 e_oemid;
        public UInt16 e_oeminfo;
        public UInt16 e_res2_0;
        public UInt16 e_res2_1;
        public UInt16 e_res2_2;
        public UInt16 e_res2_3;
        public UInt16 e_res2_4;
        public UInt16 e_res2_5;
        public UInt16 e_res2_6;
        public UInt16 e_res2_7;
        public UInt16 e_res2_8;
        public UInt16 e_res2_9;
        public UInt32 e_ifanew;
    }
}