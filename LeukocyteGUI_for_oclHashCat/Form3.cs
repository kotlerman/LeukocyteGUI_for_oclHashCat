﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeukocyteGUI_for_oclHashCat
{
    public partial class MainForm : Form
    {
        CrackTaskManager tskManager;
        public string DateTimeFormat;
        private System.Timers.Timer timer;

        public MainForm()
        {
            InitializeComponent();

            DateTimeFormat = "dd-MM-yyyy HH:mm:ss";

            tskManager = new CrackTaskManager();
            tskManager.TaskAdded += tskManager_TaskAdded;
            tskManager.TaskDeleted += tskManager_TaskDeleted;
            tskManager.TaskMovedToEnd += tskManager_TaskMovedToEnd;
            tskManager.TaskMovedToStart += tskManager_TaskMovedToStart;
            tskManager.TaskUpdated += tskManager_TaskUpdated;
            tskManager.TasksAllDeleted += tskManager_TasksAllDeleted;

            timer = new System.Timers.Timer(2000);
            timer.SynchronizingObject = this;
            timer.Elapsed += timer_Elapsed;
            timer.AutoReset = true;

            tskManager.Cracker.OnStart += Cracker_OnStart;
            tskManager.Cracker.OnStop += Cracker_OnStop;

            VisualizeTasks();
            CheckButtons();
        }

        private void Cracker_OnStop(object sender, int TaskId)
        {
            timer.Stop();
            VisualizeTask(TaskId);
        }

        private void Cracker_OnStart(object sender, int TaskId)
        {
            timer.Start();
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            VisualizeTask(MainCrackTaskManager.Cracker.CrackingTaskId);
            labelGPUSpeed.Text = MainCrackTaskManager.Cracker.Speed;
            labelGPUTemp.Text = MainCrackTaskManager.Cracker.Temp.ToString() + " °C";
            labelGPUUtil.Text = MainCrackTaskManager.Cracker.Util.ToString() + "%";
            labelFanSpeed.Text = MainCrackTaskManager.Cracker.Fan.ToString() + "%";
        }

        private void tskManager_TaskAdded(object sender, int TaskId)
        {
            VisualizeNewTask();
        }

        private void tskManager_TaskDeleted(object sender, int TaskId)
        {
            VisualizeTasks();
            CheckButtons();
        }

        private void tskManager_TaskMovedToEnd(object sender, int OriginalId, int NewId)
        {
            VisualizeTask(OriginalId);
            VisualizeTask(NewId);
        }

        private void tskManager_TaskMovedToStart(object sender, int OriginalId, int NewId)
        {
            VisualizeTask(OriginalId);
            VisualizeTask(NewId);
        }

        private void tskManager_TaskUpdated(object sender, int TaskId)
        {
            VisualizeTask(TaskId);
        }

        private void tskManager_TasksAllDeleted(object sender)
        {
            VisualizeTasks();
            CheckButtons();
        }

        private void buttonAddTask_Click(object sender, EventArgs e)
        {
            TaskEditorForm TaskEditor = new TaskEditorForm();
            TaskEditor.Owner = this;

            if (TaskEditor.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                listViewTasks.Items[listViewTasks.Items.Count - 1].Selected = true;
            }

            CheckButtons();
        }

        private void buttonOpenConverter_Click(object sender, EventArgs e)
        {
            ConverterForm Converter = new ConverterForm();
            Converter.ShowDialog(this);
        }

        public CrackTaskManager MainCrackTaskManager
        {
            get
            {
                return tskManager;
            }

            set
            {
                tskManager = value;
            }
        }

        private void buttonChangeTask_Click(object sender, EventArgs e)
        {
            if (listViewTasks.SelectedIndices.Count > 0)
            {
                int index = listViewTasks.SelectedIndices[0];
                TaskEditorForm TaskEditor = new TaskEditorForm(listViewTasks.SelectedIndices[0]);
                TaskEditor.Owner = this;
                TaskEditor.ShowDialog(this);
                listViewTasks.Items[index].Focused = true;
                listViewTasks.Items[index].Selected = true;
            }
        }

        private void buttonDeleteTask_Click(object sender, EventArgs e)
        {
            if (listViewTasks.SelectedIndices.Count > 0)
            {
                int index = listViewTasks.SelectedIndices[0];

                MainCrackTaskManager.DeleteTask(index);
                VisualizeTasks();

                if (index > 0)
                {
                    listViewTasks.Items[index - 1].Focused = true;
                    listViewTasks.Items[index - 1].Selected = true;
                }
                else if (listViewTasks.Items.Count > 0)
                {
                    listViewTasks.Items[index].Focused = true;
                    listViewTasks.Items[index].Selected = true;
                }
            }

            CheckButtons();
        }

        private void buttonClearTask_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("All tasks will be deleted. Are you sure?", "Warning",
                MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                MainCrackTaskManager.DeleteAllTasks();
                VisualizeTasks();
            }

            CheckButtons();
        }

        private void buttonUpTask_Click(object sender, EventArgs e)
        {
            if (listViewTasks.SelectedIndices.Count > 0)
            {
                int index1 = listViewTasks.SelectedIndices[0];
                int index2 = MainCrackTaskManager.TaskMoveToStart(index1);

                VisualizeTask(index1);
                VisualizeTask(index2);
                listViewTasks.Items[index2].Focused = true;
                listViewTasks.Items[index2].Selected = true;
            }
        }

        private void buttonDownTask_Click(object sender, EventArgs e)
        {
            if (listViewTasks.SelectedIndices.Count > 0)
            {
                int index1 = listViewTasks.SelectedIndices[0];
                int index2 = MainCrackTaskManager.TaskMoveToEnd(index1);

                VisualizeTask(index1);
                VisualizeTask(index2);
                listViewTasks.Items[index2].Focused = true;
                listViewTasks.Items[index2].Selected = true;
            }
        }

        private void CheckButtons()
        {
            if (listViewTasks.Items.Count > 0)
            {
                buttonClearTask.Enabled = true;
            }
            else
            {
                buttonClearTask.Enabled = false;
            }

            if (listViewTasks.SelectedIndices.Count == 1)
            {
                buttonDeleteTask.Enabled = true;
                buttonChangeTask.Enabled = true;

                if (listViewTasks.SelectedIndices[0] > 0)
                {
                    buttonUpTask.Enabled = true;
                }
                else
                {
                    buttonUpTask.Enabled = false;
                }

                if (listViewTasks.SelectedIndices[0] < listViewTasks.Items.Count - 1)
                {
                    buttonDownTask.Enabled = true;
                }
                else
                {
                    buttonDownTask.Enabled = false;
                }
            }
            else
            {
                buttonUpTask.Enabled = false;
                buttonDownTask.Enabled = false;
                buttonDeleteTask.Enabled = false;
                buttonChangeTask.Enabled = false;
            }
        }

        private void listViewTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckButtons();
        }

        public void VisualizeTask(int TaskId)
        {
            string[] values = new string[listViewTasks.Columns.Count];
            CrackTaskManager.CrackTask curTask = tskManager.CrackTasks[TaskId];

            values[0] = (TaskId + 1).ToString();
            values[1] = curTask.HashTypeName;
            values[2] = curTask.Hash;
            values[3] = curTask.Plain;
            values[4] = curTask.CurrentLength.ToString() + "/"
                + curTask.MaxLength.ToString();
            values[5] = curTask.Progress.ToString("F") + " %";
            values[6] = curTask.RecoveredDigests.ToString() + "/"
                + curTask.Digests;
            values[7] = curTask.RecoveredSalts.ToString() + "/"
                + curTask.Salts;
            values[8] = curTask.Status;
            values[9] = curTask.Estimated;
            values[10] = curTask.OutputFileName;

            switch (curTask.AttackType)
            {
                case 0:
                    values[11] = curTask.Dictionary;
                    break;
                case 3:
                    values[11] = curTask.BruteforceMask;
                    break;
            }

            values[12] = curTask.Started.ToString(DateTimeFormat);
            values[13] = curTask.Finished.ToString(DateTimeFormat);
            values[14] = curTask.SessionId;

            listViewTasks.Items[TaskId] = new ListViewItem(values);
        }

        public void VisualizeTasks()
        {
            listViewTasks.Items.Clear();

            for (int i = 0; i < tskManager.CrackTasks.Length; i++)
            {
                listViewTasks.Items.Add("");
                VisualizeTask(i);
            }
        }

        public void VisualizeNewTask()
        {
            listViewTasks.Items.Add("");
            VisualizeTask(tskManager.CrackTasks.Length - 1);
        }

        private void buttonStartTask_Click(object sender, EventArgs e)
        {
            if (listViewTasks.SelectedIndices.Count > 0)
            {
                tskManager.Cracker.SetHashcat(textBoxHashcat.Text, true);
                tskManager.Cracker.Crack(listViewTasks.SelectedIndices[0]);
            }
        }

        private void buttonPauseTask_Click(object sender, EventArgs e)
        {
            tskManager.Cracker.PauseCracking();
        }

        private void buttonStopTask_Click(object sender, EventArgs e)
        {
            if (listViewTasks.SelectedIndices.Count > 0)
            {
                tskManager.Cracker.StopCracking(listViewTasks.SelectedIndices[0]);
            }
        }
    }
}