#pragma once
#include <Arduino.h>

class CodeReader {
    public:
    CodeReader(uint8_t* codeBuffer);

    uint8_t nextByte();
    uint16_t nextShort();
    int16_t nextSignedShort();
    uint32_t nextInt();
    char* nextString();
    uint32_t position();

    private:
    uint8_t* m_codeBuf;
    uint32_t m_position;
};