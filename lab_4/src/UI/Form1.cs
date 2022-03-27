#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms;

namespace lab_4
{
    public partial class Form1 : Form
    {
        

        private int _currentStationIndex = -1;

        private int CurrentStationIndex
        {
            get { return this._currentStationIndex; }
            set
            {
                this.LastCurrentStationIndex = this._currentStationIndex < 0 & value > -1
                    ? this.LastCurrentStationIndex
                    : this._currentStationIndex;
                this._currentStationIndex = value;
            }
        }

        public int LastCurrentStationIndex { get; private set; } = -1;
        private string? _currentParamName;
        private int _currentParamIndex = -1;
        private (int SelectionStart, int Length, char LastChar) _pointerPositionControl = (0, 0, '-');

        public Form1()
        {
            try
            {
                CurrentStationIndex = -1;
                InitializeComponent();
                TreeNode parentNode = CreateTree(typeof(AbstractAts));
                
                treeView1.Nodes.Add(parentNode);

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Что-то пошло не так...", ex.ToString());
            }
        }

        private TreeNode CreateTree(Type parentType, TreeNode? parentNode = null)
        {
            
            TreeNode newNode = new TreeNode(parentType.Name);
            parentNode?.Nodes.Add(newNode);
            Type ourtype = parentType; // Базовый тип
            IEnumerable<Type> list = Assembly.GetAssembly(ourtype).GetTypes().Where(type => type.IsSubclassOf(ourtype));  // using System.Linq

            foreach(Type itm in list)
            {
                CreateTree(itm, newNode);
            }

            return parentNode ?? newNode;
        }


        private void SelectStation_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                if (sender is ListBox stationsListBox)
                {
                    IEnumerable<string> createParams;
                    CurrentStationIndex = stationsListBox.SelectedIndex;
                    listBox2.SelectedIndex = -1;
                    if (LastCurrentStationIndex != CurrentStationIndex)
                    {
                        // textBox1.Text = "";
                        _pointerPositionControl = (0, 0, '-');
                        textBox1.ReadOnly = true;
                    }

                    // _currentStationIndex == -1 при удалении выделенного объекта из списка
                    if (CurrentStationIndex > -1)
                    {
                        label3.Text = AbstractAts.AllObjects[CurrentStationIndex].ToLongString();
                        (createParams = AbstractAts.AllObjects[(int) CurrentStationIndex]
                                .ParamsAsStrings())
                            .Take(listBox2.Items.Count)
                            .Zip(Enumerable.Range(0, listBox2.Items.Count), (tStationParam, index) =>
                                listBox2.Items[index] = tStationParam.ToString())
                            .ToList();
                        createParams.Skip(listBox2.Items.Count).Select(x => listBox2.Items.Add(x)).ToList();
                        for (var i = listBox2.Items.Count - 1; i >= createParams.Count(); i--)
                        {
                            listBox2.Items.RemoveAt(i);
                        }
                    }
                    else
                    {
                        label3.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Что-то пошло не так...", ex.ToString());
            }
        }

        private void createStationButton_Click(object sender, EventArgs e)
        {
            AddStation();
        }

        private void deleteStationButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentStationIndex != null && CurrentStationIndex != -1)
                {
                    RemoveStation((int) CurrentStationIndex);
                    for (var i = listBox2.Items.Count - 1; i > -1; i--)
                    {
                        listBox2.Items.RemoveAt(i);
                    }

                    textBox1.Text = "";
                    _currentParamName = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Что-то пошло не так...", ex.ToString());
            }
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {
            // throw new System.NotImplementedException();
        }

        private void selectedParam_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (sender is ListBox paramsListBox)
                {
                    _currentParamIndex = paramsListBox.SelectedIndex;
                    // _currentStationIndex == -1 при удалении выделенного объекта из списка
                    if (_currentParamIndex > -1)
                    {
                        if (paramsListBox.Items[_currentParamIndex] is string currentParam)
                        {
                            if (CurrentStationIndex > -1)
                            {
                                textBox1.ReadOnly = false;
                                string? paramValue = AbstractAts.AllObjects[(int) CurrentStationIndex]
                                    .GetSomeValue(_currentParamName = currentParam.Split('=')[0])?.ToString();
                                if (paramValue != null & paramValue != textBox1.Text)
                                {
                                    int add;
                                    if (textBox1.Text.Length < paramValue.Length)
                                    {
                                        add = 1;
                                    }
                                    else if (textBox1.SelectionStart > 0 & textBox1.Text[textBox1.SelectionStart - 1] ==
                                             paramValue[textBox1.SelectionStart - 1])
                                    {
                                        add = -1;
                                    }
                                    else
                                    {
                                        add = 0;
                                    }

                                    _pointerPositionControl = (_pointerPositionControl.SelectionStart + add,
                                        paramValue.Length,
                                        textBox1.SelectionStart > 0 ? textBox1.Text[textBox1.SelectionStart - 1] : '-');

                                    textBox1.Text = paramValue;

                                    textBox1.SelectionStart = _pointerPositionControl.SelectionStart;
                                    // _pointerPositionControl = (textBox1.SelectionStart, textBox1.Text.Length, textBox1.SelectionStart>0?textBox1.Text[textBox1.SelectionStart-1]:'-');
                                }
                                else if (paramValue == null)
                                {
                                    textBox1.Text = "";
                                    _pointerPositionControl = (0, 0, '-');
                                }
                            }
                            else
                            {
                                // textBox1.Text = "";
                                // _currentParamName = null;
                            }
                        }
                        else
                        {
                            // textBox1.Text = "";
                            // _currentParamName = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Что-то пошло не так...", ex.ToString());
            }
        }

        private void changeSomethingParam_TextBox1(object sender, EventArgs e)
        {
            try
            {
                if (sender is TextBox currentTextBox)
                {
                    string? newValue = textBox1.Text;
                    if (CurrentStationIndex > -1 && _currentParamIndex > -1)
                    {
                        if (newValue == "")
                        {
                            newValue = null;
                            // AbstractAts.AllObjects[_currentStationIndex].SetSomeValue(_currentParamName, null);
                        }
                        // else
                        // {
                        //     
                        // }

                        if (AbstractAts.AllObjects[CurrentStationIndex].SetSomeValue(_currentParamName, newValue))
                        {
                            var lastCurrentParamIndex = (int) _currentParamIndex;
                            listBox1.Items[CurrentStationIndex] = AbstractAts.AllObjects[CurrentStationIndex];
                            listBox1.SelectedIndex = CurrentStationIndex;
                            label3.Text = AbstractAts.AllObjects[CurrentStationIndex].ToLongString();
                            _currentParamIndex = lastCurrentParamIndex;
                            if (_currentParamIndex > -1)
                            {
                                listBox2.Items[_currentParamIndex] =
                                    AbstractAts.AllObjects[CurrentStationIndex].ParamsAsStrings().ToList()[
                                        _currentParamIndex];
                                listBox2.SelectedIndex = _currentParamIndex;
                            }
                        }
                        // textBox1.Text = newValue;
                    }
                    // SelectStation_SelectedIndexChanged_1(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Что-то пошло не так...", ex.ToString());
            }
        }

        private bool AddStation(AbstractAts? newStation = null)
        {
            try
            {
                newStation ??= new AbstractAts();
                listBox1.Items.Add(newStation);
                textBox2.Text = AbstractAts.ObjectCounter.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Что-то пошло не так...", ex.ToString());
            }

            return false;
        }

        private void RemoveStation(int index)
        {
            AbstractAts? deletingStation = null;
            try
            {
                deletingStation = AbstractAts.AllObjects[index];
                AbstractAts.AllObjects.RemoveAt(index);
                listBox1.Items.RemoveAt(index);
            }
            finally
            {
                deletingStation?.Dispose();
                textBox2.Text = AbstractAts.ObjectCounter.ToString();
            }
        }

        private void RemoveStation(AbstractAts currentStation)
        {

            try
            {
                AbstractAts.AllObjects.Remove(currentStation);
                listBox1.Items.Remove(currentStation);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Что-то пошло не так...", ex.ToString());
            }
            finally
            {
                currentStation.Dispose();
            }
        }
    }
}