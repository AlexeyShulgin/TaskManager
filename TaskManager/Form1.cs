using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Thread t = null;
        int n = 5000;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            t = new Thread(Thr);
            t.Start();
        }
        public void Thr()
        {
            while (true)
            {
                try
                {
                    Process[] mas_proc = Process.GetProcesses();
                    foreach (Process proc in mas_proc)
                    {
                        try
                        {
                            this.listView1.Items.Add(new ListViewItem(new string[] { proc.ProcessName.ToString(), proc.BasePriority.ToString(), proc.Handle.ToString(), proc.HandleCount.ToString(), proc.Id.ToString(), proc.MachineName.ToString(), proc.MainModule.EntryPointAddress.ToString(), proc.MainWindowHandle.ToString(), proc.MainWindowTitle.ToString(), proc.Modules.Count.ToString(), proc.Responding.ToString(), proc.StartTime.ToString(), proc.TotalProcessorTime.ToString(), proc.Threads.Count.ToString() }));
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    Thread.Sleep(n);
                    this.listView1.Items.Clear();
                }
                catch(Exception)
                {
                    return;
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if( t != null )
                t.Abort();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.Filter = "Текстовые файлы (*.txt)|*.txt";
            if (sd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Stream stream = sd.OpenFile();
                StreamWriter sw = new StreamWriter(stream);
                try
                {
                    Process[] mas_proc = Process.GetProcesses();
                    foreach (Process proc in mas_proc)
                    {
                        try
                        {
                            sw.WriteLine("=========={0}==========", proc.ProcessName);
                            sw.WriteLine("Приоритет: {0}", proc.BasePriority);
                            sw.WriteLine("Дескриптор: {0}", proc.Handle);
                            sw.WriteLine("Количество дескрипторов: {0}", proc.HandleCount);
                            sw.WriteLine("ID: {0}", proc.Id);
                            sw.WriteLine("Имя ПК: {0}", proc.MachineName);
                            sw.WriteLine("Базовый адрес загрузки: {0}", proc.MainModule.EntryPointAddress);
                            sw.WriteLine("Дескриптор окна: {0}", proc.MainWindowHandle);
                            sw.WriteLine("Заголовок окна: {0}", proc.MainWindowTitle);
                            sw.WriteLine("Модулей: {0}", proc.Modules.Count);
                            foreach (ProcessModule pm in proc.Modules)
                            {
                                try
                                {
                                    sw.WriteLine("\tИмя модуля: {0}", pm.FileName);
                                    sw.WriteLine("\t\tАдрес модуля: {0}", pm.BaseAddress);
                                    sw.WriteLine("\t\t\tПроизводитель модуля: {0}\r\n", pm.FileVersionInfo.CompanyName);
                                }
                                catch (Exception ex)
                                {
                                    sw.WriteLine("Ошибка {0} ({1})", ex.Message, proc.ProcessName);
                                }
                            }
                            sw.WriteLine("Отвечает ли интерфейс: {0}", proc.Responding);
                            sw.WriteLine("Время запуска процесса: {0}", proc.StartTime.ToString());
                            sw.WriteLine("Процессорное время: {0}", proc.TotalProcessorTime.ToString());
                            sw.WriteLine("Количество потоков: {0}", proc.Threads.Count);
                            foreach (ProcessThread pt in proc.Threads)
                            {
                                try
                                {
                                    sw.WriteLine("\tАдрес: {0};\r\n\t\t ID: {1}\r\n", pt.StartAddress, pt.Id);
                                }
                                catch (Exception ex)
                                {
                                    sw.WriteLine("Ошибка {0} ({1})", ex.Message, proc.ProcessName);
                                }
                            }
                            sw.WriteLine("\n\n");
                        }
                        catch (Exception ex)
                        {
                            sw.WriteLine("Ошибка {0} ({1})", ex.Message, proc.ProcessName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка");
                }
                stream.Close();
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
