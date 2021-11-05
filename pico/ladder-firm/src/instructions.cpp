#include "instructions.h"

#define INTERACTION_ENABLED

namespace Instructions {

bool run(uint8_t opcode, CodeReader* reader) 
{
    switch (opcode) {
        case 0x00:
            nop(reader);
            return true;
        case 0x01:
            kbpress(reader);
            return true;
        case 0x02:
            kbrelease(reader);
            return true;
        case 0x03:
            kbreleaseall(reader);
            return true;
        case 0x04:
            mspress(reader);
            return true;
        case 0x05:
            msrelease(reader);
            return true;
        case 0x06:
            msmove(reader);
            return true;
        case 0x07:
            mswheel(reader);
            return true;
        case 0x08:
            wait(reader);
            return true;
        case 0x09:
            type(reader);
            return true;
        default:
            return false;
    }
}

void nop(CodeReader* reader) 
{

}

void kbpress(CodeReader* reader) 
{
    uint8_t keycode = reader->nextByte();

    #ifdef INTERACTION_ENABLED
    Keyboard.press(keycode);
    //Keyboard.flush();
    #endif
}

void kbrelease(CodeReader* reader) 
{
    uint8_t keycode = reader->nextByte();

    #ifdef INTERACTION_ENABLED
    Keyboard.release(keycode);
    //Keyboard.flush();
    #endif
}

void kbreleaseall(CodeReader* reader) 
{
    #ifdef INTERACTION_ENABLED
    Keyboard.releaseAll();
    //Keyboard.flush();
    #endif
}

void mspress(CodeReader* reader) 
{
    uint8_t keycode = reader->nextByte();

    #ifdef INTERACTION_ENABLED
    Mouse.press(keycode);
    Keyboard.flush();
    #endif
}

void msrelease(CodeReader* reader) 
{
    uint8_t keycode = reader->nextByte();

    #ifdef INTERACTION_ENABLED
    Mouse.release(keycode);
    Keyboard.flush();
    #endif
}

void msmove(CodeReader* reader) 
{
    int16_t dx = reader->nextSignedShort();
    int16_t dy = reader->nextSignedShort();

    #ifdef INTERACTION_ENABLED
    Mouse.move(dx, dy);
    Keyboard.flush();
    #endif
}

void mswheel(CodeReader* reader) 
{
    int16_t wheel = reader->nextSignedShort();

    #ifdef INTERACTION_ENABLED
    Mouse.move(0, 0, wheel);
    Keyboard.flush();
    #endif
}

void wait(CodeReader* reader) 
{
    uint32_t ms = reader->nextInt();

    delay(ms);
}

void type(CodeReader* reader) 
{
    char* string = reader->nextString();
    uint8_t length = string[0];
    string++;

    #ifdef INTERACTION_ENABLED
    Keyboard.print((const char*)string);
    #endif
}


}