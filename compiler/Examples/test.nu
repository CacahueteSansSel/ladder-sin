// Test code

macro run text test
keyboard_press [MENU]
wait 10
keyboard_press [R]
wait 10
type text
wait 10
end

keyboard_press [ENTER]
wait 900
keyboard_release
wait 200
mouse_move 5 -5
wait 100
// Right click
mouse_press 1
wait 100
mouse_release
exit