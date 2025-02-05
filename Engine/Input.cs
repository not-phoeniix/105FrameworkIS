using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine;

/// <summary>
/// Represents the method of input in the input system
/// </summary>
public enum InputMode
{
    Keyboard,
    Gamepad
}

/// <summary>
/// A static utility class to help with input, has a few useful
/// extra methods and updating for previous/current frame states
/// </summary>
public static class Input
{
    private static KeyboardState kbState;
    private static KeyboardState kbStatePrev;
    private static MouseState mState;
    private static MouseState mStatePrev;
    private static GamePadState padState;
    private static GamePadState padStatePrev;
    private static InputMode modePrev;

    /// <summary>
    /// Gets the current state, whether or not keyboard/gamepad is being used
    /// </summary>
    public static InputMode Mode { get; private set; }

    /// <summary>
    /// Gets current mouse state's screen position
    /// </summary>
    public static Vector2 MousePos => mState.Position.ToVector2();

    /// <summary>
    /// Gets whether or not the mouse state has changed since last frame
    /// </summary>
    public static bool MouseHasChanged => mState != mStatePrev;

    /// <summary>
    /// Gets whether or not the keyboard state has changed since last frame
    /// </summary>
    public static bool KeyboardHasChanged => kbState != kbStatePrev;

    /// <summary>
    /// Gets whether or not the gamepad state has changed since last frame
    /// </summary>
    public static bool GamepadHasChanged => padState != padStatePrev;

    /// <summary>
    /// Gets whether or not the input mode has changed since last frame
    /// </summary>
    public static bool ModeHasChanged => Mode != modePrev;

    /// <summary>
    /// Updates all input logic for input manager
    /// </summary>
    public static void Update()
    {
        // update individual input states
        kbStatePrev = kbState;
        mStatePrev = mState;
        padStatePrev = padState;
        kbState = Keyboard.GetState();
        mState = Mouse.GetState();
        padState = GamePad.GetState(0);

        // update class's internal state, only updates when a state is changed
        //   (therefore a button/key has been pressed, mouse moved, stick flicked, etc)
        if (kbState != kbStatePrev || mState != mStatePrev)
        {
            Mode = InputMode.Keyboard;
        }
        else if (
            padState.Buttons != padStatePrev.Buttons ||
            padState.ThumbSticks != padStatePrev.ThumbSticks
        )
        {
            Mode = InputMode.Gamepad;
        }

        modePrev = Mode;
    }

    /// <summary>
    /// Returns boolean whether or not a key is pressed this current frame
    /// </summary>
    /// <param name="key">Key to check down state for</param>
    /// <returns>True if key is down, false if not</returns>
    public static bool IsKeyDown(Keys key)
    {
        return kbState.IsKeyDown(key);
    }

    /// <summary>
    /// Returns boolean whatehr or not any modifier keys
    /// (shift, ctrl, alt) are pressed in this current frame
    /// </summary>
    /// <returns>True if a modifier key is pressed, false if not</returns>
    public static bool AnyModifierDown()
    {
        return kbState.IsKeyDown(Keys.LeftControl) ||
               kbState.IsKeyDown(Keys.RightControl) ||
               kbState.IsKeyDown(Keys.LeftShift) ||
               kbState.IsKeyDown(Keys.RightShift) ||
               kbState.IsKeyDown(Keys.LeftAlt) ||
               kbState.IsKeyDown(Keys.RightAlt);
    }

    /// <summary>
    /// Boolean of whether or not left mouse button is down this current frame
    /// </summary>
    /// <returns>True if mouse is down, false if not</returns>
    public static bool IsLeftMouseDown()
    {
        return mState.LeftButton == ButtonState.Pressed;
    }

    /// <summary>
    /// Boolean of whether or not right mouse button is down this current frame
    /// </summary>
    /// <returns>True if mouse is down, false if not</returns>
    public static bool IsRightMouseDown()
    {
        return mState.RightButton == ButtonState.Pressed;
    }

    /// <summary>
    /// Boolean of whether or not middle mouse button is down this current frame
    /// </summary>
    /// <returns>True if mouse is down, false if not</returns>
    public static bool IsMiddleMouseDown()
    {
        return mState.MiddleButton == ButtonState.Pressed;
    }

    /// <summary>
    /// Returns boolean whether or not a gamepad button is pressed this current frame
    /// </summary>
    /// <param name="button">Button to check down state for</param>
    /// <returns>True if button is down, false if not</returns>
    public static bool IsButtonPressed(Buttons button)
    {
        return padState.IsButtonDown(button);
    }

    /// <summary>
    /// Non-repeating key detecting, only true for one frame
    /// </summary>
    /// <param name="key">Key to check for down state</param>
    /// <returns>True if current state is down, previous state is up, false if not</returns>
    public static bool IsKeyDownOnce(Keys key)
    {
        return kbState.IsKeyDown(key) && !kbStatePrev.IsKeyDown(key);
    }

    /// <summary>
    /// Non-repeating left mouse button detecting, only true for one frame
    /// </summary>
    /// <returns>True if currently clicked and previous is released, false if not</returns>
    public static bool IsLeftMouseDownOnce()
    {
        return mState.LeftButton == ButtonState.Pressed && mStatePrev.LeftButton == ButtonState.Released;
    }

    /// <summary>
    /// Non-repeating right mouse button detecting, only true for one frame
    /// </summary>
    /// <returns>True if currently clicked and previous is released, false if not</returns>
    public static bool IsRightMouseDownOnce()
    {
        return mState.RightButton == ButtonState.Pressed && mStatePrev.RightButton == ButtonState.Released;
    }

    /// <summary>
    /// Non-repeating middle mouse button detecting, only true for one frame
    /// </summary>
    /// <returns>True if currently clicked and previous is released, false if not</returns>
    public static bool IsMiddleMouseDownOnce()
    {
        return mState.MiddleButton == ButtonState.Pressed && mStatePrev.MiddleButton == ButtonState.Released;
    }

    /// <summary>
    /// Non-repeating button checking, only true for one frame
    /// </summary>
    /// <param name="button">Button to check down state for</param>
    /// <returns>True if button is down for one frame, false if not</returns>
    public static bool IsButtonPressedOnce(Buttons button)
    {
        return padState.IsButtonDown(button) && !padStatePrev.IsButtonDown(button);
    }

    /// <summary>
    /// Returns string of all keyboard input this frame (used for text input)
    /// </summary>
    /// <returns>String concatenation of all pressed keys</returns>
    public static string GetKeyboardString()
    {
        Keys[] pressedKeys = kbState.GetPressedKeys();
        string concatenation = "";

        bool shiftPressed =
            kbState.IsKeyDown(Keys.LeftShift) ||
            kbState.IsKeyDown(Keys.RightShift);

        // decides what character to add to the string
        foreach (Keys key in pressedKeys)
        {
            if (kbStatePrev.IsKeyUp(key))
            {
                switch (key)
                {
                    case Keys.Space:
                        concatenation += " ";
                        break;

                    case Keys.OemBackslash:
                        concatenation += "\\";
                        break;

                    case Keys.OemCloseBrackets:
                        if (shiftPressed)
                        {
                            concatenation += "}";
                        }
                        else
                        {
                            concatenation += "]";
                        }
                        break;

                    case Keys.OemComma:
                        if (shiftPressed)
                        {
                            concatenation += "<";
                        }
                        else
                        {
                            concatenation += ",";
                        }
                        break;

                    case Keys.OemMinus:
                        if (shiftPressed)
                        {
                            concatenation += "_";
                        }
                        else
                        {
                            concatenation += "-";
                        }
                        break;

                    case Keys.OemOpenBrackets:
                        if (shiftPressed)
                        {
                            concatenation += "{";
                        }
                        else
                        {
                            concatenation += "[";
                        }
                        break;

                    case Keys.OemPeriod:
                        if (shiftPressed)
                        {
                            concatenation += ">";
                        }
                        else
                        {
                            concatenation += ".";
                        }
                        break;

                    case Keys.OemPipe:
                        concatenation += "|";
                        break;

                    case Keys.OemPlus:
                        if (!shiftPressed)
                        {
                            concatenation += "=";
                        }
                        else
                        {
                            concatenation += "+";
                        }
                        break;

                    case Keys.OemQuestion:
                        if (!shiftPressed)
                        {
                            concatenation += "/";
                        }
                        else
                        {
                            concatenation += "?";
                        }
                        break;

                    case Keys.OemQuotes:
                        if (!shiftPressed)
                        {
                            concatenation += "'";
                        }
                        else
                        {
                            concatenation += "\"";
                        }
                        break;

                    case Keys.OemSemicolon:
                        if (shiftPressed)
                        {
                            concatenation += ":";
                        }
                        else
                        {
                            concatenation += ";";
                        }
                        break;

                    case Keys.Tab:
                        concatenation += "\t";
                        break;

                    case Keys.D0:
                        if (!shiftPressed)
                        {
                            concatenation += "0";
                        }
                        else
                        {
                            concatenation += ")";
                        }
                        break;

                    case Keys.D1:
                        if (!shiftPressed)
                        {
                            concatenation += "1";
                        }
                        else
                        {
                            concatenation += "!";
                        }
                        break;

                    case Keys.D2:
                        if (!shiftPressed)
                        {
                            concatenation += "2";
                        }
                        else
                        {
                            concatenation += "@";
                        }
                        break;

                    case Keys.D3:
                        if (!shiftPressed)
                        {
                            concatenation += "3";
                        }
                        else
                        {
                            concatenation += "#";
                        }
                        break;

                    case Keys.D4:
                        if (!shiftPressed)
                        {
                            concatenation += "4";
                        }
                        else
                        {
                            concatenation += "$";
                        }
                        break;

                    case Keys.D5:
                        if (!shiftPressed)
                        {
                            concatenation += "5";
                        }
                        else
                        {
                            concatenation += "%";
                        }
                        break;

                    case Keys.D6:
                        if (!shiftPressed)
                        {
                            concatenation += "6";
                        }
                        else
                        {
                            concatenation += "^";
                        }
                        break;

                    case Keys.D7:
                        if (!shiftPressed)
                        {
                            concatenation += "7";
                        }
                        else
                        {
                            concatenation += "&";
                        }
                        break;

                    case Keys.D8:
                        if (!shiftPressed)
                        {
                            concatenation += "8";
                        }
                        else
                        {
                            concatenation += "*";
                        }
                        break;

                    case Keys.D9:
                        if (!shiftPressed)
                        {
                            concatenation += "9";
                        }
                        else
                        {
                            concatenation += "(";
                        }
                        break;

                    // regular keyboard keys
                    default:
                        if (key == Keys.LeftShift ||
                            key == Keys.RightShift ||
                            key.ToString().Length != 1
                        )
                        {
                            continue;
                        }

                        // modify "shiftPressed" for all regular letter
                        //   keys to be inverted if caps lock is enabled
                        bool shiftCapsMod = shiftPressed;
                        if (kbState.CapsLock)
                        {
                            shiftCapsMod = !shiftCapsMod;
                        }

                        if (shiftCapsMod)
                        {
                            concatenation += key.ToString().ToUpper();
                        }
                        else
                        {
                            concatenation += key.ToString().ToLower();
                        }
                        break;

                }
            }
        }

        return concatenation;
    }

    /// <summary>
    /// Edits an inputted string reference with internal keyboard text
    /// input (THIS INCLUDES BACKSPACE FOR DELETION)
    /// </summary>
    /// <param name="edit">String reference to edit</param>
    public static void UpdateKeyboardString(ref string edit)
    {
        bool ctrlPressed =
            kbState.IsKeyDown(Keys.LeftControl) ||
            kbState.IsKeyDown(Keys.RightControl);

        if (IsKeyDownOnce(Keys.Back) && edit.Length > 0)
        {
            if (ctrlPressed)
            {
                // find index of last space (as long as it's not a trailing space)
                int spaceIndex = edit.Length - 1;
                bool foundANonSpaceYet = false;
                char iterChar;
                do
                {
                    // track whether or not a space has been found so
                    //   strings that end in a space or three will still
                    //   remove the end word and it'll skip past those
                    //   initial spaces
                    iterChar = edit[spaceIndex];
                    if (iterChar != ' ')
                    {
                        foundANonSpaceYet = true;
                    }

                    spaceIndex--;

                    if (spaceIndex < 0)
                    {
                        // make index -2 so below it sets the substring
                        //   length to zero, removing the entire last
                        //   word (not leaving the first character)
                        spaceIndex = -2;
                        break;
                    }
                } while (iterChar != ' ' || !foundANonSpaceYet);

                // substring the inputted string to remove all characters
                //   after the space (removing that word lol)
                // (index is +2'd so one space is left behind (mirrors most OS's))
                edit = edit.Substring(0, spaceIndex + 2);

            }
            else
            {
                // if ctrl not pressed, just remove one char lol
                string modified = edit.Remove(edit.Length - 1);
                edit = modified;
            }
        }
        else
        {
            edit += GetKeyboardString();
        }
    }
}
