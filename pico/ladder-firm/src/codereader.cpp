#include "codereader.h"

CodeReader::CodeReader(uint8_t* codeBuffer) : m_codeBuf(codeBuffer), m_position(0)
{}

uint32_t CodeReader::position() 
{
    return m_position;
}

uint8_t CodeReader::nextByte() 
{
    uint8_t byte = m_codeBuf[m_position];
    m_position++;

    return byte;
}

uint16_t CodeReader::nextShort() 
{
    uint16_t value = (m_codeBuf[m_position + 1] << 8) | m_codeBuf[m_position];
    m_position += 2;

    return value;
}

int16_t CodeReader::nextSignedShort() 
{
    uint16_t value = nextShort();

    return (int16_t)value - 512;
}

uint32_t CodeReader::nextInt() 
{
    uint32_t value = (m_codeBuf[m_position + 3] << 24) | (m_codeBuf[m_position + 2] << 16) | (m_codeBuf[m_position + 1] << 8) | m_codeBuf[m_position];
    m_position += 4;

    return value;
}

char* CodeReader::nextString() 
{
    uint8_t length = nextByte();
    char* buffer = (char*)malloc(length+2);

    buffer[0] = length;
    for (uint8_t i = 0; i < length; i++) {
        buffer[i+1] = nextByte();
    }
    buffer[length+1] = '\0';

    return buffer;
}
