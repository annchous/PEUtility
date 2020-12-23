﻿namespace PEUtility.Tools
{
    public enum RelocationType
    {
        IMAGE_REL_BASED_ABSOLUTE = 0,
        IMAGE_REL_BASED_HIGH = 1,
        IMAGE_REL_BASED_LOW = 2,
        IMAGE_REL_BASED_HIGHLOW = 3,
        IMAGE_REL_BASED_HIGHADJ = 4,
        IMAGE_REL_BASED_MIPS_JMPADDR = 5,
        IMAGE_REL_BASED_ARM_MOV32 = 5,
        IMAGE_REL_BASED_RISCV_HIGH20 = 5,
        IMAGE_REL_BASED_THUMB_MOV32 = 7,
        IMAGE_REL_BASED_RISCV_LOW12I = 7,
        IMAGE_REL_BASED_RISCV_LOW12S = 8,
        IMAGE_REL_BASED_MIPS_JMPADDR16 = 9,
        IMAGE_REL_BASED_DIR64 = 10
    }
}