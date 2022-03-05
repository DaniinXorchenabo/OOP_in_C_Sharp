#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms;

namespace lab_2
{
    public partial class Form1 : Form
    {
        private List<TelephoneStation> _telephoneStations;

        private int _currentStationIndex = -1;
        private int CurrentStationIndex
        {
            get
            {
                return this._currentStationIndex;
            }
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
        

        public Form1(List<TelephoneStation> telephoneStationsList)
        {
            CurrentStationIndex = -1;
            InitializeComponent();
            _telephoneStations = telephoneStationsList;
            _telephoneStations.Select(AddStation);
            
        }


        private void SelectStation_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (sender is ListBox stationsListBox)
            {
                IEnumerable<string> createParams;
                CurrentStationIndex = stationsListBox.SelectedIndex;
                listBox2.SelectedIndex = -1;
                if (LastCurrentStationIndex != CurrentStationIndex)
                {
                    textBox1.Text = "";
                    textBox1.ReadOnly = true;
                }

                // _currentStationIndex == -1 при удалении выделенного объекта из списка
                if (CurrentStationIndex > -1)
                {
                    label3.Text = _telephoneStations[CurrentStationIndex].ToLongString();
                    (createParams = _telephoneStations[(int) CurrentStationIndex]
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

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            // throw new System.NotImplementedException();
        }

        private void createStationButton_Click(object sender, EventArgs e)
        {
            AddStation();
        }

        private void deleteStationButton_Click(object sender, EventArgs e)
        {
            if (CurrentStationIndex != null)
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

        // private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        // {
        //     // throw new System.NotImplementedException();
        // }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {
            // throw new System.NotImplementedException();
        }

        private void selectedParam_SelectedIndexChanged(object sender, EventArgs e)
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
                            object paramValue = _telephoneStations[(int) CurrentStationIndex]
                                .GetSomeValue(_currentParamName = currentParam.Split('=')[0]);
                            if (paramValue != null)
                            {
                                textBox1.Text = paramValue.ToString();
                            }
                            else
                            {
                                textBox1.Text = "";
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // throw new System.NotImplementedException();
        }

        private void changeSomethingParam_TextBox1(object sender, EventArgs e)
        {
            if (sender is TextBox currentTextBox){
                string ? newValue = textBox1.Text;
                if (CurrentStationIndex > -1 && _currentParamIndex > -1)
                {
                    if (newValue == "")
                    {
                        newValue = null;
                        // _telephoneStations[_currentStationIndex].SetSomeValue(_currentParamName, null);
                    }
                    // else
                    // {
                    //     
                    // }

                    if (_telephoneStations[CurrentStationIndex].SetSomeValue(_currentParamName, newValue))
                    {

                        var lastCurrentParamIndex = (int) _currentParamIndex;
                        listBox1.Items[CurrentStationIndex] = _telephoneStations[CurrentStationIndex];
                        listBox1.SelectedIndex = CurrentStationIndex;
                        label3.Text = _telephoneStations[CurrentStationIndex].ToLongString();
                        _currentParamIndex = lastCurrentParamIndex;
                        if (_currentParamIndex > -1)
                        {
                            listBox2.Items[_currentParamIndex] =
                                _telephoneStations[CurrentStationIndex].ParamsAsStrings().ToList()[_currentParamIndex];
                            listBox2.SelectedIndex = _currentParamIndex;
                        }
                    }
                    // textBox1.Text = newValue;
                }
                // SelectStation_SelectedIndexChanged_1(sender, e);
            }
        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {
            // throw new System.NotImplementedException();
        }

        private bool AddStation(TelephoneStation newStation = null)
        {
            _telephoneStations.Add(newStation ??= new TelephoneStation());
            listBox1.Items.Add(newStation);
            textBox2.Text = TelephoneStation.ObjectCounter.ToString();
            return false;
        }

        private void RemoveStation(int index)
        {
            TelephoneStation? deletingStation = null;
            try
            {
                deletingStation = _telephoneStations[index];
                _telephoneStations.RemoveAt(index);
                listBox1.Items.RemoveAt(index);
            }
            finally
            {
                deletingStation?.Dispose();
                textBox2.Text = TelephoneStation.ObjectCounter.ToString();
            }

        }

        private void RemoveStation(TelephoneStation currentStation)
        {
            _telephoneStations.Remove(currentStation);
            listBox1.Items.Remove(currentStation);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // throw new System.NotImplementedException();
        }
    }
}