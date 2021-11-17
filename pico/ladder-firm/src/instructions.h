#pragma once
#include <Arduino.h>
#include "tusb.h"
#include <Adafruit_TinyUSB.h>
#define USE_TINYUSB
#include "tinyusb_mouse_and_keyboard.h"
#include "tusb.h"
#include "codereader.h"

namespace Instructions {

extern const uint8_t _inst_asciimap[128];

bool run(uint8_t opcode, CodeReader* reader);

void nop(CodeReader* reader);
void kbpress(CodeReader* reader);
void kbrelease(CodeReader* reader);
void kbreleaseall(CodeReader* reader);
void mspress(CodeReader* reader);
void msrelease(CodeReader* reader);
void msmove(CodeReader* reader);
void mswheel(CodeReader* reader);
void wait(CodeReader* reader);
void type(CodeReader* reader);
void freakout(CodeReader* reader);

}