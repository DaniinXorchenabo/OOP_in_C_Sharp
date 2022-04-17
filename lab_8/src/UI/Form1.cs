using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab_8
{
public partial class Form1 : Form
    {
        private static Random _random = null!;

        private static Dictionary<TreeNode, AbstractAts> _TreeNodeToTStationObj =
            new Dictionary<TreeNode, AbstractAts>();

        private static Dictionary<TreeNode, Type> _TreeNodeToTStationClass =
            new Dictionary<TreeNode, Type>();


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

        static Form1()
        {
            _random = new Random();
        }

        public Form1()
        {
            try
            {
                CurrentStationIndex = -1;
                InitializeComponent();

                Action<AbstractAts> changeCounter;
                AbstractAts.AddItemEvent += (tStation) =>
                {
                    textBox1.ReadOnly = true;
                    // createButton.Enabled = true;c
                    createCustomizedNameButton.Enabled = false;
                    deleteButton.Enabled = false;
                    listBox2.SelectedIndex = -1;
                };
                AbstractAts.AddItemEvent += changeCounter = (tStation) =>
                {
                    textBox2.Text = AbstractAts.ObjectCounter.ToString();
                };

                AbstractAts.AddItemEvent += (tStation) =>
                {
                    if (
                        treeView1 != null
                        && treeView1.SelectedNode != null
                        && _TreeNodeToTStationClass.ContainsKey(treeView1.SelectedNode))
                    {
                        var newNode = new TreeNode(tStation.ToString());
                        treeView1.SelectedNode.Nodes.Add(newNode);
                        _TreeNodeToTStationObj[newNode] = tStation;
                    }
                };



                AbstractAts.RemoveItemEvent += changeCounter;
                AbstractAts.RemoveItemEvent += (tStation) =>
                {
                    if (_TreeNodeToTStationObj.ContainsValue(tStation))
                    {
                        var curentNode = _TreeNodeToTStationObj
                            .Where(x => x.Value == tStation)
                            .FirstOrDefault().Key;
                        _TreeNodeToTStationObj.Remove(curentNode);
                        curentNode.Parent.Nodes.Remove(curentNode);
                    }
                };


                new CoordinateStation(_random);
                new CoordinateStation(_random);
                new MachineStation(_random);

                TreeNode parentNode = CreateTree(typeof(AbstractAts));
                parentNode.ExpandAll();

                treeView1.Nodes.Add(parentNode);
                textBox1.ReadOnly = true;
                createButton.Enabled = false;
                deleteButton.Enabled = false;
                createCustomizedNameButton.Enabled = false;
                textBox2.Text = AbstractAts.ObjectCounter.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Что-то пошло не так...");
            }
        }

        private TreeNode CreateTree(Type parentType, TreeNode? parentNode = null)
        {
            TreeNode newNode = new TreeNode(parentType.Name);
            _TreeNodeToTStationClass[newNode] = parentType;
            parentNode?.Nodes.Add(newNode);
            Type ourtype = parentType; // Базовый тип
            IEnumerable<Type> list = Assembly.GetAssembly(ourtype).GetTypes()
                .Where(type => type.IsSubclassOf(ourtype)); // using System.Linq

            var isBool = true;
            foreach (var itm in list)
            {
                isBool = false;
                CreateTree(itm, newNode);
            }

            if (isBool && parentType.GetProperty("_AllObjects").GetValue(parentType) is List<AbstractAts> allObj)
            {
                foreach (var stationObj in allObj)
                {
                    TreeNode currentNode = new TreeNode(stationObj.ToString());
                    newNode.Nodes.Add(currentNode);
                    _TreeNodeToTStationObj[currentNode] = stationObj;
                }
            }

            return parentNode ?? newNode;
        }


        private void SelectStation_SelectedIndexChanged_1(object sender, TreeViewEventArgs e)
        {
            try
            {
                Type currentClass;
                TreeView? currentTreeView = sender as TreeView;
                if (
                    currentTreeView != null
                    && currentTreeView.SelectedNode != null
                    && _TreeNodeToTStationClass.ContainsKey(currentTreeView.SelectedNode)
                    && !(currentClass = _TreeNodeToTStationClass[currentTreeView.SelectedNode]).IsAbstract)
                {
                    textBox1.ReadOnly = true;
                    createButton.Enabled = true;
                    deleteButton.Enabled = false;
                    createCustomizedNameButton.Enabled = false;
                    listBox2.SelectedIndex = -1;

                    for (var i = listBox2.Items.Count - 1; i >= 0; i--)
                    {
                        listBox2.Items.RemoveAt(i);
                    }

                    textBox1.Text = "";
                }
                else if (
                    currentTreeView != null
                    && currentTreeView.SelectedNode != null
                    && _TreeNodeToTStationObj.ContainsKey(currentTreeView.SelectedNode))
                {
                    listBox2.SelectedIndex = -1;
                    createButton.Enabled = false;
                    deleteButton.Enabled = true;
                    createCustomizedNameButton.Enabled = true;
                    textBox1.Text = "";
                    textBox1.ReadOnly = true;

                    var currentObj = _TreeNodeToTStationObj[currentTreeView.SelectedNode];
                    IEnumerable<string> createParams;

                    label3.Text = currentObj.ToLongString();
                    (createParams = currentObj.ParamsAsStrings())
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
                    textBox1.ReadOnly = true;
                    createButton.Enabled = false;
                    deleteButton.Enabled = false;
                    createCustomizedNameButton.Enabled = false;
                    listBox2.SelectedIndex = -1;

                    for (var i = listBox2.Items.Count - 1; i >= 0; i--)
                    {
                        listBox2.Items.RemoveAt(i);
                    }

                    textBox1.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Что-то пошло не так...");
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
                if (
                    treeView1 != null
                    && treeView1.SelectedNode != null
                    && _TreeNodeToTStationObj.ContainsKey(treeView1.SelectedNode))
                {
                    var currentObj = _TreeNodeToTStationObj[treeView1.SelectedNode];

                    RemoveStation(currentObj);
                    for (var i = listBox2.Items.Count - 1; i > -1; i--)
                    {
                        listBox2.Items.RemoveAt(i);
                    }

                    textBox1.Text = "";
                    _currentParamName = null;
                    textBox1.ReadOnly = true;
                    createButton.Enabled = false;
                    deleteButton.Enabled = false;
                    createCustomizedNameButton.Enabled = false;
                    listBox2.SelectedIndex = -1;
                }

                ActiveControl = textBox2;
                if (treeView1 != null) treeView1.SelectedNode = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Что-то пошло не так...");
            }
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
                            if (treeView1.SelectedNode != null &&
                                _TreeNodeToTStationObj.ContainsKey(treeView1.SelectedNode))
                            {
                                textBox1.ReadOnly = false;
                                string? paramValue = _TreeNodeToTStationObj[treeView1.SelectedNode]
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
                MessageBox.Show(this, ex.ToString(), "Что-то пошло не так...");
            }
        }

        private void changeSomethingParam_TextBox1(object sender, EventArgs e)
        {
            try
            {
                if (sender is TextBox currentTextBox)
                {
                    string? newValue = textBox1.Text;
                    if (
                        treeView1.SelectedNode != null
                        && _TreeNodeToTStationObj.ContainsKey(treeView1.SelectedNode)
                        && _currentParamIndex > -1)
                    {
                        if (newValue == "")
                        {
                            newValue = null;
                        }

                        var currentStation = _TreeNodeToTStationObj[treeView1.SelectedNode];

                        if (currentStation.SetSomeValue(_currentParamName, newValue))
                        {
                            var lastCurrentParamIndex = (int) _currentParamIndex;

                            label3.Text = currentStation.ToLongString();
                            _currentParamIndex = lastCurrentParamIndex;
                            if (_currentParamIndex > -1)
                            {
                                listBox2.Items[_currentParamIndex] =
                                    currentStation.ParamsAsStrings().ToList()[
                                        _currentParamIndex];
                                if (treeView1.SelectedNode != null
                                    && _TreeNodeToTStationObj.ContainsKey(treeView1.SelectedNode))
                                {
                                    var currentNode = treeView1.SelectedNode;
                                    var currentObj = _TreeNodeToTStationObj[currentNode];
                                    treeView1.SelectedNode.Text = currentObj.ToString();
                                    treeView1.SelectedNode = currentNode;
                                }

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
                MessageBox.Show(this, ex.ToString(), "Что-то пошло не так...");
            }
        }

        private bool AddStation(AbstractAts? newStation = null)
        {
            try
            {
                Type currentClass;
                if (
                    treeView1 != null
                    && treeView1.SelectedNode != null
                    && _TreeNodeToTStationClass.ContainsKey(treeView1.SelectedNode)
                    && !(currentClass = _TreeNodeToTStationClass[treeView1.SelectedNode]).IsAbstract)
                {
                    AbstractAts newItem = newStation ?? (AbstractAts) Activator.CreateInstance(currentClass, _random);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Что-то пошло не так...");
            }

            return false;
        }


        private void RemoveStation(AbstractAts currentStation)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Что-то пошло не так...");
            }
            finally
            {
                currentStation.Dispose();
            }
        }


        private void createCustomizedNameButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeView1.SelectedNode != null &&
                    _TreeNodeToTStationObj.ContainsKey(treeView1.SelectedNode))
                {
                    _TreeNodeToTStationObj[treeView1.SelectedNode].CreateCustomizedName();
                    var currentStation = _TreeNodeToTStationObj[treeView1.SelectedNode];

                    var lastCurrentParamIndex = (int) _currentParamIndex;

                    label3.Text = currentStation.ToLongString();

                    int index = 0;
                    int targetIndex = 0;
                    foreach (var someParam in currentStation.ParamsAsStrings())
                    {
                        if (((someParam as string)!).StartsWith("CompanyName="))
                        {
                            targetIndex = index;
                        }

                        index++;
                    }


                    listBox2.Items[targetIndex] = currentStation.ParamsAsStrings().ToList()[targetIndex];
                    if (treeView1.SelectedNode != null
                        && _TreeNodeToTStationObj.ContainsKey(treeView1.SelectedNode))
                    {
                        var currentNode = treeView1.SelectedNode;
                        var currentObj = _TreeNodeToTStationObj[currentNode];
                        treeView1.SelectedNode.Text = currentObj.ToString();
                        treeView1.SelectedNode = currentNode;
                    }

                    listBox2.SelectedIndex = _currentParamIndex;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Что-то пошло не так...");
            }
        }
    }
}