using System.Text.Json;

namespace Task_Tracker {
    internal class Program {

        static string workingDir = "../../../";
        static void Main(string[] args) {

            string[] commandArgs = { "add", "update", "delete", "mark-in-progress", "mark-done", "list" };
            string[] status = { "todo", "in-progress", "done" };

            if (!File.Exists($"{workingDir}Task.json")) {
                File.WriteAllText($"{workingDir}Task.json", "[]");
            }
            while (true) {
                string jsonString = File.ReadAllText($"{workingDir}Task.json");
                List<Task> TaskList = JsonSerializer.Deserialize<List<Task>>(jsonString) ?? new List<Task>();

                Console.Write("task-cli> ");

                string command = Console.ReadLine()!;
                string[] arguments = command.Split(' ');

                // Add 
                if (arguments.Length > 1 && arguments[0] == commandArgs[0]) {
                    string[] descriptionArrray = arguments.Skip(1).ToArray();
                    string description = string.Join(" ", descriptionArrray);

                    var data = new Task {
                        id = TaskList.Count + 1,
                        description = description.Trim('"'),
                        status = status[0],
                        createdAt = DateTime.Now,
                        updatedAt = DateTime.Now,
                    };

                    TaskList.Add(data);
                    Console.WriteLine($"Task Updated Successfully (ID: {TaskList.Count})");
                    SaveToFile(TaskList);
                }
                else if (arguments.Length > 2 && arguments[0] == commandArgs[1]) {
                    int id = int.Parse(arguments[1]);
                    int index = TaskList.FindIndex(x => x.id == id);

                    if (index == -1) {
                        Console.WriteLine($"No Id {id} found");
                        continue;
                    }

                    string[] descriptionArray = arguments.Skip(2).ToArray();
                    string description = string.Join(" ", descriptionArray);

                    var data = new Task {
                        id = id,
                        description = description.Trim('"'),
                        status = TaskList[index].status,
                        createdAt = DateTime.Now,
                        updatedAt = DateTime.Now,
                    };

                    TaskList[index] = data;
                    Console.WriteLine($"Task Updated Successfully (ID: {id})");
                    SaveToFile(TaskList);
                }

                else if (arguments.Length > 1 && arguments[0] == commandArgs[2]) {
                    int id = int.Parse(arguments[1]);
                    int index = TaskList.FindIndex(x => x.id == id);

                    if (index == -1) {
                        Console.WriteLine($"No Id {id} found");
                        continue;

                    }
                    TaskList.RemoveAt(index);
                    SaveToFile(TaskList);
                }

                else if (arguments.Length > 1 && arguments[0] == commandArgs[3] || arguments[0] == commandArgs[4]) {
                    int id = int.Parse(arguments[1]);
                    int index = TaskList.FindIndex(x => x.id == id);

                    if (index == -1) {
                        Console.WriteLine($"No Id {id} found");
                        continue;
                    }
                    string new_status = arguments[0] == commandArgs[3] ? status[1] : status[2];

                    var data = new Task {
                        id = id,
                        description = TaskList[index].description,
                        status = new_status,
                        createdAt = DateTime.Now,
                        updatedAt = DateTime.Now,
                    };
                    TaskList[index] = data;
                    Console.WriteLine($"Task Updated Successfully (ID: {id})");
                    SaveToFile(TaskList);
                }
                else if (arguments[0] == commandArgs[5]) {
                    if (arguments.Length > 1) {
                        string list_status = arguments[1];
                        TaskList = TaskList.FindAll(x => x.status.Equals(list_status));
                    };

                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string taskString = JsonSerializer.Serialize(TaskList, options);
                    Console.WriteLine(taskString);

                }
                else {
                    Console.WriteLine("Command List: \n add <description> \n update <id> <description>\n delete <id> \n mark-in-progress <id> \n mark-done <id> \n list \n list done \n list todo \n list in-progress \n ");
                }
            }

        }

        public static void SaveToFile(List<Task> task) {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string taskString = JsonSerializer.Serialize(task, options);
            File.WriteAllText($"{workingDir}Task.json", taskString);
        }

        public class Task {
            public int id { get; set; }
            public required string description { get; set; }
            public required string status { get; set; }
            public DateTime createdAt { get; set; }
            public DateTime updatedAt { get; set; }
        }
    }
}
