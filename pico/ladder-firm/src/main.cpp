#include <Arduino.h>
#include "instructions.h"
#include "code.h"

CodeReader* reader = nullptr;
bool halted = false;

void setup() 
{
    reader = new CodeReader(CODE_BUFFER);

    TinyUSBDevice.begin();
    Keyboard.begin();
    Keyboard.print("   ");
    Mouse.begin();
}

void loop()
{
    if (halted) return;

    uint8_t opcode = reader->nextByte();

    if (opcode == 0x0a) 
    {
        halted = true;

        Keyboard.releaseAll();
        pinMode(LED_BUILTIN, OUTPUT);
        Keyboard.end();
        Mouse.end();
        free(reader);
        return;
    }

    Instructions::run(opcode, reader);
}