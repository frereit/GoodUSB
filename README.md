# GoodUSB
Simple C# Program to block new keyboard devices until Control+Alt+Delete is pressed

The idea of this program is not to 'fix' BadUSB devices (such as a RubberDucky). This only protects against general exploits/scripts and does not protect against targeted attacks or repeated attacks. 

# Installation

Start exe with admin privileges

# Notes

I chose to only wait for Control+Alt+Delete and not have the user enter a pin or password simply because it was enough for my purposes and it was easy to implement it this way.

Feel free to fork, modify and create pull requests with improvements.

Here are some ideas that I am too lazy to implement:

* block only input from the new keyboard
* force the user to input a random generated pin on the new keyboard to unlock it. This would be a great improvement as it would force rubber ducky scripts to parse the screen and figure out the pin
* NSIS installer that puts the program in startup with admin privileges automatically



The source is very small so you should look at it to confirm its not malware.