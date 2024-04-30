namespace DiplayUsbKeyLogging
{
    public partial class Form1 : Form
    {
        public class WindowKeyEvent
        {
            public string OpenedApplication { get; set; }
            public string OpenedWindow { get; set; }
            public string KeyPressed { get; set; }

            public WindowKeyEvent(string openedApplication, string openedWindow, string keyPressed)
            {
                OpenedApplication = openedApplication;
                OpenedWindow = openedWindow;
                KeyPressed = keyPressed;
            }
        }
        List<WindowKeyEvent> events;
        List<WindowKeyEvent> filteredEvents;
        public Form1()
        {
            InitializeComponent();
            this.events = new List<WindowKeyEvent>();
         filteredEvents = new List<WindowKeyEvent>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
            FilterData();
          //  addDataToDGV(events);
            addDataToDGV(filteredEvents);
        }
        private void FilterData()
        {
           

            if (events.Count > 0)
            {
                WindowKeyEvent currentEvent = events[0];
                if (currentEvent.KeyPressed.Length > 1)
                {
                    currentEvent.KeyPressed = "[" + currentEvent.KeyPressed + "]";
                }

                for (int i = 1; i < events.Count; i++)
                {
                    WindowKeyEvent nextEvent = events[i];
                   
                    // Check if the next event is for the same tab of the same window
                    if (currentEvent.OpenedApplication == nextEvent.OpenedApplication &&
                        currentEvent.OpenedWindow == nextEvent.OpenedWindow)
                    {
                        
                        // Combine the key pressed
                        if (nextEvent.KeyPressed.Length > 1)
                        {
                            currentEvent.KeyPressed += "[" + nextEvent.KeyPressed + "]";
                        }
                        else
                        {
                            currentEvent.KeyPressed += nextEvent.KeyPressed;
                        }
                    }
                    else
                    {
                        // Add the current event to the filtered list
                        filteredEvents.Add(currentEvent);
                        // Move to the next event
                        currentEvent = nextEvent;
                        if (currentEvent.KeyPressed.Length > 1)
                        {
                            currentEvent.KeyPressed = "[" + currentEvent.KeyPressed + "]";
                        }

                    }
                }

                // Add the last event to the filtered list
                filteredEvents.Add(currentEvent);
            }

            // Now, filteredEvents contains combined data of the same tab of the same window
            // You can use filteredEvents for further processing
        }



        private void LoadData()
        {
            // Read data from the text file
            string[] lines = File.ReadAllLines("data.txt");

            // Parse each line and create WindowKeyEvent objects
            foreach (string l in lines)
            {
                // Split the line based on the separator " :  "
                string[] parts = l.Split(new string[] { " :  " }, StringSplitOptions.None);

                // Extracting opened application, opened window, and pressed key
                string line = parts[0].Substring(parts[0].IndexOf("[") + 1, parts[0].IndexOf("]") - parts[0].IndexOf("[") - 1);
                string[] windowAndApplication = line.Split(new string[] { " - " }, StringSplitOptions.None);

                // Extracting opened window
                string openedWindow = string.Join(" - ", windowAndApplication.Take(windowAndApplication.Length - 1));

                // Extracting opened application
                string openedApplication = windowAndApplication.Last();

                // Extracting pressed key
                string keyPressed = parts[1].Substring(parts[1].IndexOf("[") + 1, parts[1].IndexOf("]") - parts[1].IndexOf("[") - 1);
                 keyPressed = keyPressed.Trim(); // Assuming the key pressed is always a single character


                // Create a WindowKeyEvent object and add it to the list
                WindowKeyEvent windowKeyEvent = new WindowKeyEvent(openedApplication, openedWindow, keyPressed);
                events.Add(windowKeyEvent);
            }

            
            
        }
        private void addDataToDGV(List<WindowKeyEvent> data)
        {
            dgv.Dock = DockStyle.Fill; // Fill the DataGridView to the form
            dgv.AutoGenerateColumns = true; // Automatically generate columns based on properties of WindowKeyEvent
            dgv.DataSource = data;
        }
    }
}
