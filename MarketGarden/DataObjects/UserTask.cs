using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class UserTask
    {
        public User Sender { get; private set; }
        public int UserID_assignee { get; private set; }
        public DateTime AssignDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public string TaskName { get; private set; }
        public string TaskDescription { get; private set; }
        public bool Finished { get; private set; }

        public UserTask(User sender, int userID_assignee, 
            DateTime assignDate, DateTime dueDate, 
            string taskName, string taskDescription)
        {
            this.Sender = sender;
            this.UserID_assignee = userID_assignee;
            this.AssignDate = assignDate;
            this.DueDate = dueDate;
            this.TaskName = taskName;
            this.TaskDescription = taskDescription;
        }
    }
}
