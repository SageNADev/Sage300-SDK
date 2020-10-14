// The MIT License (MIT) 
// Copyright (c) 1994-2020 The Sage Group plc or its licensors.  All rights reserved.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of 
// this software and associated documentation files (the "Software"), to deal in 
// the Software without restriction, including without limitation the rights to use, 
// copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the 
// Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


#region Namespaces
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard.Properties;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard
{
    /// <summary> UI for UI Generation Wizard </summary>
    public partial class UIGeneration : Form
    {
        #region Private Classes
        /// <summary> Class for information stored per cell </summary>
        class CellInfo
        {
            /// <summary> Column for cell </summary>
            public int ColIndex { get; set; }
            /// <summary> Row for cell </summary>
            public int RowIndex { get; set; }
            /// <summary> Control name in cell </summary>
            public string Name { get; set; }
            /// <summary> Data grid </summary>
            public DataGridView Control { get; set; }
        }
        #endregion

        #region Private vars
        /// <summary> List of controls </summary>
        private readonly Dictionary<string, ControlInfo> _controlsList = new Dictionary<string, ControlInfo>();
        /// <summary> Save off mouse down </summary>
        private bool _mouseDown;
        /// <summary> Save off dragging point </summary>
        private Point _draggingFromPoint;
        /// <summary> Save off dragging object </summary>
        private object _dragObject;
        /// <summary> Global for control type selected </summary>
        private ControlType _controlType = ControlType.None;
        /// <summary> Global for selected control </summary>
        private Control _selectedControl = null;
        /// <summary> Global for cell information </summary>
        private CellInfo _cellInfo = null;
        /// <summary> List of entities </summary>
        private List<BusinessView> _entities = null;
        /// <summary> Context Menu for widgets </summary>
        private readonly ContextMenu _contextMenu = new ContextMenu();
        /// <summary> Menu Item for Dropdown </summary>
        private readonly MenuItem _dropDownMenuItem = new MenuItem() { Text = Resources.Dropdown, Tag = WidgetDropDown };
        /// <summary> Menu Item for Radio Buttons </summary>
        private readonly MenuItem _radioButtonsMenuItem = new MenuItem() { Text = Resources.RadioButtons, Tag = WidgetRadioButtons };
        #endregion

        #region Private Enums
        /// <summary> Type of controls to be added in wizard </summary>
        private enum ControlType
        {
            /// <summary> No control </summary>
            None = 0,
            /// <summary> Tab control </summary>
            Tab = 1,
            /// <summary> Label control (business property) </summary>
            Label = 2,
            /// <summary> Grid control </summary>
            Grid = 3
        }
        #endregion

        #region Private Constants
        private const string WidgetDropDown = "Dropdown";
        private const string WidgetRadioButtons = "RadioButtons";
        private const string WidgetNumeric = "Numeric";
        private const string WidgetTextbox = "Textbox";
        private const string WidgetFinder = "Finder";
        private const string WidgetDateTime = "DateTime";
        private const string WidgetCheckbox = "Checkbox";
        private const string WidgetTime = "Time";
        private const string WidgetTab = "Tab";
        private const string WidgetTabPage = "TabPage";
        private const string WidgetGrid = "Grid";

        private const string PrefixPalette = "palette";
        private const string PrefixColumn = "Column";
        private const string PrefixTab = "tabStrip";
        private const string PrefixGrid = "grid";

        private const string SuffixTabTage = "_page";

        private const string NodeEntities = "entities";
        private const string NodeLayout = "Layout";
        private const string NodeControls = "Controls";
        private const string NodeControl = "Control";

        private const string AttributeType = "type";
        private const string AttributeRewRow = "newRow";
        private const string AttributeWidget = "widget";
        private const string AttributeEntity = "entity";
        private const string AttributeProperty = "property";
        private const string AttributeId = "id";
        private const string AttributeDiv = "div";
        private const string AttributeLi = "li";
        private const string AttributeTrue = "true";
        private const string AttributeFalse = "false";
        private const string AttributeText = "text";

        #endregion

        public UIGeneration()
        {
            InitializeComponent();
            CreatePalette(splitDesigner.Panel1);
            InitEvents();
        }

        #region Public Properties
        public XDocument XMLLayout = null;
        public Dictionary<string, List<string>> Widgets = new Dictionary<string, List<string>>();
        #endregion

        /// <summary> Initialize events for process generation class </summary>
        private void InitEvents()
        {
            // Context Events
            _dropDownMenuItem.Click += MenuItemOnClick;
            _radioButtonsMenuItem.Click += MenuItemOnClick;
        }

        /// <summary> Set widget type for selected control</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="eventArgs">Event Args </param>
        private void MenuItemOnClick(object sender, EventArgs eventArgs)
        {
            _controlsList[_selectedControl.Name].Widget = ((MenuItem)sender).Tag.ToString();
        }

        /// <summary> Create palette </summary>
        /// <param nam="parent">Parent control</param>
        private void CreatePalette(Control parent)
        {
            // Create control
            var control = new DataGridView();

            //Set properties
            control.Name = GetUniqueControlName(PrefixPalette);
            control.AllowDrop = true;
            control.AllowUserToAddRows = false;
            control.AllowUserToDeleteRows = false;
            control.AllowUserToResizeColumns = false;
            control.AllowUserToResizeRows = false;
            control.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            control.ColumnHeadersVisible = false;
            control.DefaultCellStyle.BackColor = control.Name.EndsWith("1") ? SystemColors.Window : Color.PaleTurquoise;
            control.DefaultCellStyle.SelectionBackColor = SystemColors.Window;
            control.ScrollBars = ScrollBars.None;

            // Add the new control
            AddNewControl(control, parent);

            control.Dock = DockStyle.Fill;
            control.EditMode = DataGridViewEditMode.EditProgrammatically;
            control.MultiSelect = false;
            control.ReadOnly = true;
            control.RowHeadersVisible = false;
            control.RowHeadersWidth = 62;
            control.RowTemplate.Height = 18;

            // Add columns to fit size of parent control
            var cols = control.Width / 108;
            for (int i = 1; i <= cols; i++)
            {
                control.Columns.Add(PrefixColumn + i, "");
                control.Columns[i - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            // Add rows to fit size of parent control
            control.Rows.Add(control.Height / control.RowTemplate.Height);

            // Clear selection
            control.ClearSelection();

            // Add events
            control.DragEnter += DragEnterHandler;
            control.DragDrop += DragDropHandler;
            control.CellClick += PaletteCellClickHandler;

        }

        /// <summary> Initialize properties </summary>
        /// <param name="control">Selected control</param>
        private void InitProperties(Control control)
        {
            btnDelete.Enabled = false;
            btnAddPage.Enabled = false;

            // No control so nothing is selected
            if (control == null)
            {
                _controlType = ControlType.None;
                _selectedControl = null;
            }
            else
            {
                btnDelete.Enabled = true;

                // Determine type of control selected
                var type = control.GetType();

                // Special case for tab control
                if (type == typeof(TabControl))
                {
                    _controlType = ControlType.Tab;
                    _selectedControl = ((TabControl)control).SelectedTab;
                    type = _selectedControl.GetType();
                }
                else
                {
                    _selectedControl = control;
                }

                if (type == typeof(TabPage))
                {
                    _controlType = ControlType.Tab;
                    btnAddPage.Enabled = true;
                }
                else if (type == typeof(DataGridView))
                {
                    _controlType = ControlType.Tab;
                    _selectedControl = control.Parent;
                    btnAddPage.Enabled = true;
                }
                else if (type == typeof(FlowLayoutPanel))
                {
                    _controlType = ControlType.Grid;
                }
                else
                {
                    _controlType = ControlType.Label;
                }
            }

            // Init properties
            InitTextProp();
            InitControlProp();
        }

        /// <summary> Initialize Text Property </summary>
        private void InitTextProp()
        {
            if (_controlType == ControlType.None)
            {
                // No text property
                txtPropText.Text = string.Empty;
                txtPropText.ReadOnly = true;
            }
            else if (_controlType == ControlType.Grid)
            {
                // Text property
                txtPropText.Text = GetControlInfo(_selectedControl.Name).Text;
                txtPropText.ReadOnly = false;
            }
            else
            {
                // Can only edit text for a non-label (business property)
                txtPropText.Text = _selectedControl.Text;
                txtPropText.ReadOnly = _controlType == ControlType.Label;
            }
        }

        /// <summary> Initialize Control Property </summary>
        private void InitControlProp()
        {
            if (_controlType == ControlType.None || _controlType != ControlType.Label)
            {
                // No control is selected or control is not a label
                txtPropWidget.Text = string.Empty;
            }
            else
            {
                // Get business property to get data type
                var controlInfo = GetControlInfo(_selectedControl.Name);
                var value = string.Empty;

                switch (controlInfo.BusinessField.Type)
                {
                    case BusinessDataType.Double:
                    case BusinessDataType.Long:
                    case BusinessDataType.Integer:
                    case BusinessDataType.Decimal:
                    case BusinessDataType.Short:
                        value = WidgetNumeric;
                        break;

                    case BusinessDataType.String:
                        value = controlInfo.BusinessField.IsKey ? WidgetFinder  : WidgetTextbox;
                        break;

                    case BusinessDataType.DateTime:
                        value = WidgetDateTime;
                        break;

                    case BusinessDataType.Boolean:
                    case BusinessDataType.Byte:
                        value = WidgetCheckbox;
                        break;

                    case BusinessDataType.TimeSpan:
                        value = WidgetTime;
                        break;

                    case BusinessDataType.Enumeration:
                        value = WidgetDropDown;
                        break;

                    default:
                        break;
                }

                // Set widget type based upon data type
                if (string.IsNullOrEmpty(controlInfo.Widget))
                {
                    controlInfo.Widget = value;
                }

                txtPropWidget.Text = controlInfo.Widget;
            }

            txtPropWidget.ReadOnly = true;

        }

        /// <summary>
        /// Assign events
        /// </summary>
        private void AssignEvents()
        {
            // Mouse handler events
            MouseHandlerEvents(treeEntities, "");
            MouseHandlerEvents(picTab, Resources.TabControl);
            MouseHandlerEvents(picGrid, Resources.GridContainer);

            // Main palette
            splitDesigner.Panel1.DragEnter += DragEnterHandler;
            splitDesigner.Panel1.DragDrop += DragDropHandler;
        }

        /// <summary>
        /// Assign events for mouse Up/Move 
        /// </summary>
        /// <param name="control">Control</param>
        /// <param name="tooltip">Tool tip</param>
        private void MouseHandlerEvents(Control control, string toolTip)
        {
            // Toolbox and tree view (properties) mouse move
            control.MouseUp += MouseUpHandler;
            control.MouseMove += MouseMoveHandler;
            control.MouseDown += MouseDragHandler;

            // Tooltip
            if (!string.IsNullOrEmpty(toolTip))
            {
                tooltip.SetToolTip(control, toolTip);
            }
        }

        /// <summary>
        /// Get control information
        /// </summary>
        /// <param name="key">Key to collection</param>
        /// <returns>User Selection</returns>
        private ControlInfo GetControlInfo(string key)
        {
            if (_controlsList.ContainsKey(key))
            {
                return _controlsList[key];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get Cell Info from object
        /// </summary>
        /// <param name="control">Control object to evaluate</param>
        private CellInfo GetCellInfo(object control)
        {
            CellInfo cellInfo = null;

            // Get cell info
            if (control.GetType() == typeof(Label) || control.GetType() == typeof(TabControl) || control.GetType() == typeof(FlowLayoutPanel))
            {
                // In a grid (grid!)
                if (((Control)control).Parent.GetType() == typeof(FlowLayoutPanel))
                {
                    return cellInfo;
                }

                // Get grid control to look at cells
                var grid = (DataGridView)((Control)control).Parent;

                // Get CellInfo object stored in tag
                var tag = (CellInfo)((Control)control).Tag;
                for (int row = 0; row < grid.Rows.Count; row++)
                {
                    for (int col = 0; col < grid.Columns.Count; col++)
                    {
                        if (grid[col, row].Tag != null && ((CellInfo)grid[col, row].Tag).Name == tag.Name)
                        {
                            cellInfo = new CellInfo()
                            {
                                ColIndex = col,
                                RowIndex = row,
                                Name = tag.Name,
                                Control = grid
                            };
                            break;
                        }
                    }
                    if (cellInfo != null)
                    {
                        break;
                    }
                }
            }

            return cellInfo;
        }

        /// <summary>
        /// Set dragging objects
        /// </summary>
        /// <param name="dragObject">Drag Object</param>
        /// <param name="e">Mouse args</param>
        private void SetDraggingObjects(object dragObject, MouseEventArgs e)
        {
            _mouseDown = true;
            _dragObject = dragObject;
            _draggingFromPoint = new Point(e.X, e.Y);
            _cellInfo = GetCellInfo(dragObject);
        }

        /// <summary>
        /// Do Drag Handler
        /// </summary>
        private void DoDragHandler()
        {
            // Sets object to start the drag
           _mouseDown = false;
            DoDragDrop(_dragObject, DragDropEffects.Move);
        }

        /// <summary>
        /// Allows mouse down to start move from added fields area (layout)
        /// </summary>
        /// <param name="sender">Control</param>
        /// <param name="e">Mouse args</param>
        private void MouseDownHandler (object sender, MouseEventArgs e)
        {
            // Sets the object in the drag object
            SetDraggingObjects(sender, e);
        }

        /// <summary>
        /// Allows mouse down to start move from added fields area (layout)
        /// </summary>
        /// <param name="sender">Control</param>
        /// <param name="e">Mouse args</param>
        private void MouseUpHandler(object sender, MouseEventArgs e)
        {
            _mouseDown = false;
        }

        /// <summary>
        /// Allows mouse move to start move
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Mouse args</param>
        private void MouseMoveHandler(object sender, MouseEventArgs e)
        {
            if (_mouseDown)
            {
                if (Math.Abs(e.X - _draggingFromPoint.X) >= 5 ||
                    Math.Abs(e.Y - _draggingFromPoint.Y) >= 5)
                {
                    DoDragHandler();
                }
            }
        }

        /// <summary>
        /// Allows mouse down to initiate drag from tree/toolbox
        /// </summary>
        /// <param name="sender">Control</param>
        /// <param name="e">Mouse args</param>
        private void MouseDragHandler(object sender, MouseEventArgs e)
        {
            object dragObject = ((Control)sender).Tag;
            if (sender.GetType() == typeof(TreeView))
            {
                // Sets the business field object in the drag object
                var node = treeEntities.GetNodeAt(e.X, e.Y);
                // Check to ensure that node was clicked on
                if (node != null)
                {
                    var controlInfo = GetControlInfo(node.Name);
                    if (controlInfo != null && controlInfo.BusinessField != null)
                    {
                        controlInfo.Node = node;
                        controlInfo.ParentNodeName = node.Parent.Name;
                        dragObject = controlInfo;
                    }
                    else
                    {
                        dragObject = null;
                    }
                }
                else
                {
                    dragObject = null;
                }
            }

            // Sets the drag object
            if (dragObject != null)
            {
                SetDraggingObjects(dragObject, e);
            }
        }

        /// <summary>
        /// Allows click event to be common
        /// </summary>
        /// <param name="sender">Control</param>
        /// <param name="e">Event args</param>
        private void ClickHandler(object sender, EventArgs e)
        {
            // Init properties of clicked widget
            InitProperties((Control)sender);

            // Determine if context menu is requested for a label (business property)
            if (sender.GetType() == typeof(Label))
            {
                // Determine type of widget for business property
                var controlinfo = GetControlInfo(_selectedControl.Name);
                if (controlinfo.Widget == WidgetDropDown || controlinfo.Widget == WidgetRadioButtons)
                {
                    var mouseEventArgs = (MouseEventArgs)e;
                    if (mouseEventArgs != null && mouseEventArgs.Button == MouseButtons.Right)
                    {
                        // Clear, build, and show context menu
                        _contextMenu.MenuItems.Clear();

                        _dropDownMenuItem.Checked = controlinfo.Widget == WidgetDropDown;
                        _radioButtonsMenuItem.Checked = controlinfo.Widget == WidgetRadioButtons;

                        _contextMenu.MenuItems.Add(_dropDownMenuItem);
                        _contextMenu.MenuItems.Add(_radioButtonsMenuItem);

                        _contextMenu.Show((Control)sender, mouseEventArgs.Location);
                    }
                }
            }
        }

        /// <summary>
        /// Cell Info for Drop
        /// </summary>
        /// <param name="control">Control being dropped on</param>
        /// <param name="hitTestInfo">Hit test in grid</param>
        /// <param name="point">Point object</param>
        /// <param name="name">Name to be used in tag property</param>
        private CellInfo CellInfoForDrop(Control control, DataGridView.HitTestInfo hitTestInfo, ref Point point, string name)
        {
            CellInfo cellInfo = null;

            // If dropping in grid, then get cell info for targetted cell
            // Adjust X, Y to be start of cell
            point.X = hitTestInfo.ColumnX;
            point.Y = hitTestInfo.RowY;

            // Local reference
            var gridControl = (DataGridView)control;

            // Can't drop into an occupied cell
            if (gridControl[hitTestInfo.ColumnIndex, hitTestInfo.RowIndex].Tag != null)
            {
                return null;
            }

            // Set cell info object to be set in tag
            cellInfo = new CellInfo()
            {
                ColIndex = hitTestInfo.ColumnIndex,
                RowIndex = hitTestInfo.RowIndex,
                Control = gridControl,
                Name = name
            };
            gridControl[hitTestInfo.ColumnIndex, hitTestInfo.RowIndex].Tag = cellInfo;

            return cellInfo;
        }

        /// <summary>
        /// Cell Info for Move
        /// </summary>
        /// <param name="control">Control being dropped on</param>
        /// <param name="hitTestInfo">Hit test in grid</param>
        /// <param name="point">Point object</param>
        /// <param name="movingControl">Control being moved</param>
        private CellInfo CellInfoForMove(Control control, DataGridView.HitTestInfo hitTestInfo, ref Point point, Control movingControl)
        {
            CellInfo cellInfo = null;

            // Adjust X, Y for cell start, if applicable
            if (hitTestInfo != null)
            {
                point.X = hitTestInfo.ColumnX;
                point.Y = hitTestInfo.RowY;
            }

            // Local reference
            var gridControl = control.GetType() != typeof(FlowLayoutPanel) ? (DataGridView)control: null;

            // Info from previous and destination cell
            var fromCellInfo = _cellInfo;
            var toCellInfo = gridControl != null ? gridControl[hitTestInfo.ColumnIndex, hitTestInfo.RowIndex].Tag : null;
            if (toCellInfo != null && fromCellInfo.Name == ((CellInfo)toCellInfo).Name)
            {
                // Moving to itself
                return null;
            }
            else if (toCellInfo != null)
            {
                // Can't move to an occupied cell
                return null;
            }

            // Moving from a cell or a grid
            var name = fromCellInfo == null ? movingControl.Name : fromCellInfo.Name;

            // Set tag in destination cell and unset in origination cell
            cellInfo = new CellInfo()
            {
                ColIndex = hitTestInfo != null ? hitTestInfo.ColumnIndex : 0,
                RowIndex = hitTestInfo != null ? hitTestInfo.RowIndex : 0,
                Control = gridControl,
                Name = name
            };

            if (gridControl != null)
            {
                gridControl[hitTestInfo.ColumnIndex, hitTestInfo.RowIndex].Tag = cellInfo;
            }

            if (_cellInfo != null)
            {
                _cellInfo.Control[_cellInfo.ColIndex, _cellInfo.RowIndex].Tag = null;
            }

            movingControl.Tag = cellInfo;

            return cellInfo;
        }

        /// <summary>
        /// Dropping field or moving existing field in the added fields area (layout)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DragDropHandler(object sender, DragEventArgs e)
        {
            var controlInfo = (ControlInfo)e.Data.GetData(typeof(ControlInfo));
            var label = (Label)e.Data.GetData(typeof(Label));
            var tabControl = (TabControl)e.Data.GetData(typeof(TabControl));
            var toolboxControl = (string)e.Data.GetData(DataFormats.Text);
            var flowPanel = (FlowLayoutPanel)e.Data.GetData(typeof(FlowLayoutPanel));
            var control = (Control)sender;
            var type = control.GetType();
            DataGridView.HitTestInfo hitTestInfo = null;
            CellInfo cellInfo = null;

            // If dropping a tab,
            if (toolboxControl == WidgetTab)
            {
                // Can only drop on a data grid view
                if  (type != typeof(DataGridView))
                {
                    return;
                }
                // Can't drop on a data grid view if it's parent is a tab
                else if (control.Parent != null && control.Parent.GetType() == typeof(TabPage))
                {
                    return;
                }

                // Can only have 1 tab control
                if (DoesTabExist())
                {
                    return;
                }
            }

            // If moving a tab, can't move on another tab
            if (tabControl != null && type == typeof(DataGridView))
            {
                if (control.Parent != null && control.Parent.GetType() == typeof(TabPage))
                {
                    return;
                }
            }

            // If dropping a grid, can only drop on a cell and not on another grid
            if (toolboxControl == WidgetGrid)
            {
                // Can only drop on a data grid view
                if (type != typeof(DataGridView))
                {
                    return;
                }
            }

            // Get point where field is being dropped or moved to
            var point = control.PointToClient(new Point(e.X, e.Y));

            // If dropping in grid, then get cell info for targetted cell
            if (type == typeof(DataGridView))
            {
                hitTestInfo = ((DataGridView)control).HitTest(point.X, point.Y);
            }

            // Is it is a new business field being dropped?
            if (controlInfo != null)
            {
                if (type == typeof(DataGridView))
                {
                    cellInfo = CellInfoForDrop(control, hitTestInfo, ref point, controlInfo.ParentNodeName + "_" + controlInfo.BusinessField.Name);

                    // Can't drop into an occupied cell
                    if (cellInfo == null)
                    {
                        return;
                    }

                }

                // Do not add if it has already been added. No reason for adding
                // twice in a simplistic layout. Can do manually if needed
                if (controlInfo.Node.ForeColor == Color.Green)
                {
                    return; 
                }

                CreateLabel(controlInfo, point, control, cellInfo);

            }
            else if (!string.IsNullOrEmpty(toolboxControl) && toolboxControl == WidgetTab)
            {
                if (type == typeof(DataGridView))
                {
                    cellInfo = CellInfoForDrop(control, hitTestInfo, ref point, WidgetTab);

                    // Can't drop into an occupied cell
                    if (cellInfo == null)
                    {
                        return;
                    }
                }

                CreateTab(point, control, cellInfo);
            }
            else if (!string.IsNullOrEmpty(toolboxControl) && toolboxControl == WidgetGrid)
            {
                if (type == typeof(DataGridView))
                {

                    cellInfo = CellInfoForDrop(control, hitTestInfo, ref point, WidgetGrid);

                    // Can't drop into an occupied cell
                    if (cellInfo == null)
                    {
                        return;
                    }
                }

                CreateGrid(point, control, cellInfo);
        }
            else if (label != null || tabControl != null)
            {
                Control movingControl = label != null ? label : (Control)tabControl;
                cellInfo = CellInfoForMove(control, hitTestInfo, ref point, movingControl);
                
                // Can't move
                if (cellInfo == null)
                {
                    return;
                }

                MoveControl(movingControl, point, control, e);
            }
            else if (flowPanel != null)
            {
                cellInfo = CellInfoForMove(control, hitTestInfo, ref point, flowPanel);

                // Can't move
                if (cellInfo == null)
                {
                    return;
                }
                
                MoveControl(flowPanel, point, control, e);
            }
        }

        /// <summary>
        /// Create a label
        /// </summary>
        /// <param name="controlInfo"></param>
        /// <param name="point">Location to create</param>
        /// <param name="destinationControl">Destination Control</param>
        /// <param name="cellInfo">Cell info for tag property</param>
        /// <returns>Created control</returns>
        private Control CreateLabel(ControlInfo controlInfo, Point point, Control destinationControl, CellInfo cellInfo)
        {
            // Create a new label to represent the field being added to the layout
            var control = new Label
            {
                Name = controlInfo.ParentNodeName + "_" + controlInfo.BusinessField.Name,
                Text = controlInfo.BusinessField.Name,
                Location = point,
                ForeColor = Color.FromArgb(0, 0, 255),
                AutoSize = true,
                Tag = cellInfo,
                BackColor = Color.Transparent
            };

            // Add the handlers
            AddHandlers(control);

            // Remove node from the tree
            // treeEntities.Nodes.Remove(controlInfo.Node);
            // Change color of node instead of removing
            controlInfo.Node.ForeColor = Color.Green;

            AddNewControl(control, destinationControl);

            return control;
        }

        /// <summary>
        /// Create a tab
        /// </summary>
        /// <param name="point">Location to create</param>
        /// <param name="destinationControl">Destination Control</param>
        /// <param name="cellInfo">Cell info for tag property</param>
        /// <returns>Created control</returns>
        private Control CreateTab(Point point, Control destinationControl, CellInfo cellInfo)
        {
            var name = GetUniqueControlName(PrefixTab);

            if (cellInfo != null)
            {
                cellInfo.Name = name;
            }

            // Create a new tab in the layout
            var parentControl = new TabControl
            {
                Name = name,
                Location = point,
                Size = new Size(destinationControl.Width - point.X - 2, 250),
                Tag = cellInfo
            };
            parentControl.TabPages.Clear();

            var control = new TabPage
            {   
                Name = name + SuffixTabTage + "1",
                Text = WidgetTab + "1",
                Tag = 1,
                BackColor = Color.White
            };

            // Add the handlers
            AddHandlers(parentControl, true);

            // Add the page to the control
            parentControl.TabPages.Add(control);

            // Add the newly created control
            AddNewControl(parentControl, destinationControl, control);

            // Create palette for this tab page
            CreatePalette(control);

            return parentControl;
        }

        /// <summary>
        /// Create a grid
        /// </summary>
        /// <param name="point">Location to create</param>
        /// <param name="destinationControl">Destination Control</param>
        /// <param name="cellInfo">Cell info for tag property</param>
        /// <returns>Created control</returns>
        private Control CreateGrid(Point point, Control destinationControl, CellInfo cellInfo)
        {
            var name = GetUniqueControlName(PrefixGrid);

            if (cellInfo != null)
            {
                cellInfo.Name = name;
            }

            // Create a new flow panel (grid) in the layout
            var control = new FlowLayoutPanel
            {
                Name = name,
                Location = point,
                Size = new Size(destinationControl.Width - point.X - 2, 36),
                AllowDrop = true,
                BorderStyle = BorderStyle.FixedSingle,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.PaleGreen,
                Tag = cellInfo
            };

            // Add handlers
            AddHandlers(control, true);

            // Add the newly created control
            AddNewControl(control, destinationControl);

            return control;
        }

        /// <summary>
        /// Get unique control name
        /// </summary>
        /// <param name="prefix">Prefix</param>
        private string GetUniqueControlName(string prefix)
        {
            // Init
            string retVal = "";

            // Create iteration to generate unique name
            for (int i = 1; i < 1000; i++)
            {
                // Build value
                retVal = prefix + i.ToString();

                if (!_controlsList.ContainsKey(retVal))
                {
                    return retVal;
                }
            }
            // Fail safe
            return retVal;
        }

        /// <summary>
        /// Does tab already exist?
        /// </summary>
        private bool DoesTabExist()
        {
            // Init
            var retVal = false;

            // Iterate collection looking for a tab control
            foreach (var controlInfo in _controlsList.Values)
            {
                if (controlInfo.Control != null && controlInfo.Control.GetType() == typeof(TabControl))
                {
                    retVal = true;
                    break;
                }
            }

            return retVal;
        }

        /// <summary>
        /// Get a new tab page
        /// </summary>
        /// <param name="control"></param>
        private TabPage GetTabPage(TabControl tabControl)
        {
            // Init
            string name = "";
            string text = "";

            for (int i = 1; i < 20; i++)
            {
                // Increment and set values to search
                name = tabControl.Name + SuffixTabTage + i.ToString();
                text = WidgetTab + i.ToString();

                // Name does not exist, so use this one
                if (!tabControl.TabPages.ContainsKey(name))
                {
                    break;
                }
            }

            // Create a new tab page
            var control = new TabPage
            {
                Name = name,
                Text = text,
                Tag = tabControl.TabPages.Count + 1
            };

            return control;
        }

        /// <summary>
        /// Add handlers
        /// </summary>
        /// <param name="control">Control to set events on</param>
        /// <param name="draggable">True if draggable otherwise false</param>
        private void AddHandlers(Control control, bool draggable = false)
        {
            control.MouseDown += MouseDownHandler;
            control.MouseUp += MouseUpHandler;
            control.MouseMove += MouseMoveHandler;
            control.Click += ClickHandler;

            if (draggable)
            {
                control.DragEnter += DragEnterHandler;
                control.DragDrop += DragDropHandler;
            }
        }

        /// <summary>
        /// Remove handlers
        /// </summary>
        /// <param name="control"></param>
        /// <param name="draggable"></param>
        private void RemoveHandlers(Control control, bool draggable = false)
        {
            control.MouseDown -= MouseDownHandler;
            control.MouseUp -= MouseUpHandler;
            control.MouseMove -= MouseMoveHandler;
            control.Click -= ClickHandler;

            if (draggable)
            {
                control.DragEnter -= DragEnterHandler;
                control.DragDrop -= DragDropHandler;
            }
        }

        /// <summary>
        /// Add new control
        /// </summary>
        /// <param name="control">Control to create</param>
        /// <param name="destinationControl">Destination Control on layout</param>
        /// <param name="childControl">Child control if tab Control</param>
        private void AddNewControl(Control control, Control destinationControl, Control childControl = null)
        {
            // Do not add labels to control list if a business property
            if (control.GetType() != typeof(Label))
            {
                _controlsList.Add(control.Name, new ControlInfo() { Control = control });
            }

            // Add the newly created control
            destinationControl.Controls.Add(control);

            if (destinationControl.GetType() != typeof(FlowLayoutPanel))
            {
                control.BringToFront();
            }

            if (childControl != null)
            {
                ClickHandler(childControl, null);
            }
            else
            {
                ClickHandler(control, null);
            }
        }

        /// <summary>
        /// Move a control
        /// </summary>
        /// <param name="control"></param>
        /// <param name="point"></param>
        /// <param name="destinationControl"></param>
        /// <param name="e">Drag Evnts Args</param>
        private void MoveControl(Control control, Point point, Control destinationControl, DragEventArgs e)
        {
            // Ownership is not changing
            if (control.Parent == destinationControl)
            {
                control.Location = point;
            }
            // Moving, but dropping on itself. No ownership change
            else if (control == destinationControl ||
                (control.GetType() == typeof(TabControl) &&
                destinationControl.GetType() == typeof(TabPage) && control == destinationControl.Parent))
            {
                control.Location = control.Parent.PointToClient(new Point(e.X, e.Y));
            }
            // Moving to another container
            else if (control.Parent != destinationControl)
            {
                // Do not move to another parent if the new parent is a grid and the 
                // control being moved is not a label
                if (destinationControl.GetType() == typeof(FlowLayoutPanel) && control.GetType() != typeof(Label))
                {
                    // Do not move
                    return;
                }

                // Remove from old parent and assign to new parent
                control.Parent.Controls.Remove(control);
                destinationControl.Controls.Add(control);
                control.Location = point;
            }
            else
            {
                control.Location = new Point(e.X, e.Y);
            }

            // Adjust width if non-label
            if (control.GetType() != typeof(Label))
            {
                control.Width = destinationControl.Width - point.X - 2;

                // Need to adjust data view control if tab
                if (control.GetType() == typeof(TabControl))
                {
                    // Adjust height first
                    if (control.Height + point.Y + 8 < destinationControl.Height)
                    {
                        control.Height = 250;
                    }
                    else  if (control.Height + point.Y > destinationControl.Height)
                    {
                        control.Height = destinationControl.Height - point.Y - 8;
                    }

                    RealignLabels(control);
                }
            }
            control.Refresh();
        }

        private void RealignLabels(Control tabControl)
        {
            // Iterate tab pages of tab control
            foreach (Control control in tabControl.Controls)
            {
                // If nested tab control, do recursion
                if (control.GetType() == typeof(TabControl))
                {
                    RealignLabels(control);
                }
                // If tab page, look at controls
                if (control.GetType() == typeof(TabPage))
                {
                    // Iterate controls in tab page
                    foreach (Control child in control.Controls)
                    {
                        // If data grid view, look to align labels
                        if (child.GetType() == typeof(DataGridView))
                        {
                            // Local grid reference
                            var grid = (DataGridView)child;

                            // Get number of columns and rows that new grid can support
                            var supportedCols = grid.Width / 108;
                            var supportedRows = grid.Height / grid.RowTemplate.Height;
                            // Get the current number of columns and rows
                            var currentCols = grid.Columns.Count;
                            var currentRows = grid.Rows.Count;
                            // Get the larger column and row count (grid shrunk or grew)
                            var maxCols = Math.Max(currentCols, supportedCols);
                            var maxRows = Math.Max(currentRows, supportedRows);

                            // Adjust rows if necessary
                            if (supportedRows != currentRows)
                            {
                                for (int i = maxRows; i >= 1; i--)
                                {
                                    // Did grid grow?
                                    if (i > currentRows)
                                    {
                                        grid.Rows.Add();
                                    }
                                    // Did grid shrink?
                                    if (i > supportedRows)
                                    {
                                        // Delete any controls in this row first
                                        for (int col = 0; col < grid.Columns.Count; col++)
                                        {
                                            // Delete control if one found in the cell
                                            if (grid[col, i - 1].Tag != null)
                                            {
                                                var cellInfo = (CellInfo)grid[col, i - 1].Tag;
                                                var controlToDelete = grid.Controls[cellInfo.Name];
                                                DeleteControl(controlToDelete);
                                            }
                                        }

                                        // Delete row
                                        grid.Rows.RemoveAt(i - 1);
                                    }
                                }
                                // Clear selection
                                grid.ClearSelection();
                            }

                            // Adjust columns if necessary
                            if (supportedCols != currentCols)
                            {
                                for (int i = maxCols; i >= 1; i--)
                                {
                                    // Did grid grow?
                                    if (i > currentCols)
                                    {
                                        var col = grid.Columns.Add(PrefixColumn + i, "");
                                        grid.Columns[col].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                                    }
                                    // Did grid shrink?
                                    if (i > supportedCols)
                                    {
                                        // Delete any controls in this column first
                                        for (int row = 0; row < grid.Rows.Count; row++)
                                        {
                                            // Delete control if one found in the cell
                                            if (grid[i - 1, row].Tag != null)
                                            {
                                                var cellInfo = (CellInfo)grid[i - 1, row].Tag;
                                                var controlToDelete = grid.Controls[cellInfo.Name];
                                                DeleteControl(controlToDelete);
                                            }
                                        }

                                        // Delete column
                                        grid.Columns.RemoveAt(i - 1);
                                    }
                                }
                                // Clear selection
                                grid.ClearSelection();
                            }

                            // Iterate children in grid, if any
                            foreach (Control gridControl in grid.Controls)
                            {
                                // If it is a label
                                if (gridControl.GetType() == typeof(Label))
                                {
                                    // Assign local reference and get tag (cellinfo)
                                    var label = (Label)gridControl;
                                    var cellInfo = (CellInfo)label.Tag;

                                    // Get rectangle of cell for new label position
                                    var rectangle = grid.GetCellDisplayRectangle(cellInfo.ColIndex, cellInfo.RowIndex, false);
                                    label.Location = new Point(rectangle.Left + grid.Left, rectangle.Top + grid.Top);
                                    label.Refresh();
                                }
                                else if (gridControl.GetType() == typeof(FlowLayoutPanel))
                                {
                                    // Assign local reference and get tag (cellinfo)
                                    var gridLayout = (FlowLayoutPanel)gridControl;
                                    var cellInfo = (CellInfo)gridLayout.Tag;

                                    // Get rectangle of cell for new grid position
                                    var rectangle = grid.GetCellDisplayRectangle(cellInfo.ColIndex, cellInfo.RowIndex, false);
                                    gridLayout.Location = new Point(rectangle.Left + grid.Left, rectangle.Top + grid.Top);
                                    gridLayout.Size = new Size(grid.Width - rectangle.Left - 2, 36);
                                    gridLayout.Refresh();
                                }
                            }
                        }

                    }
                }

            }

        }
        /// <summary>
        /// Delete a tab
        /// </summary>
        /// <param name="control">Control to remove</param>
        private void DeleteTab(Control control)
        {
            // Get the parent
            var parentControl = (TabControl)control.Parent;

            // If any children, delete them
            DeleteChildren(control);

            // Delete tab page
            parentControl.TabPages.Remove((TabPage)control);

            // If no tab pages left, then remove control
            if (parentControl.TabPages.Count == 0)
            {
                // Remove the handlers
                RemoveHandlers(parentControl, true);

                // Remove from Layout
                RemoveFromLayout(parentControl);
                _controlsList.Remove(parentControl.Name);
            }

        }

        /// <summary>
        /// Delete a Control (grid, container, label), but not tab 
        /// </summary>
        /// <param name="control">Control to delete</param>
        private void DeleteControl(Control control)
        {
            // Gets business field object from name in the added fields list
            var controlInfo = GetControlInfo(control.Name);
            var children = control.GetType() == typeof(FlowLayoutPanel);
            var name = control.Name;

            // Remove the handlers
            RemoveHandlers(control, children);

            DeleteChildren(control);

            // Remove from Layout
            RemoveFromLayout(control);

            if (controlInfo != null && controlInfo.BusinessField != null)
            {
                // Add back to the available fields list
                controlInfo.ParentNodeName = string.Empty;

                // Set color back to window text if not in use still
                //if (!IsStillInUse((DataGridView)splitDesigner.Panel1.Controls[PrefixPalette + "1"], name))
                //{
                controlInfo.Node.ForeColor = SystemColors.WindowText;
                //}
            }

            if (controlInfo != null && controlInfo.BusinessField == null)
            {
                _controlsList.Remove(control.Name);
            }
        }

        /// <summary>
        /// Determines if a control is still in use
        /// </summary>
        /// <param name="grid">Grid/Palette to evaluate</param>
        /// <param name="name">Control name searching for</param>
        /// <returns>true if found otherwise false</returns>
        private bool IsStillInUse(DataGridView grid, string name)
        {
            var stillInUse = false;

            // Iterate grid rows
            for (int row = 0; row < grid.Rows.Count; row++)
            {
                // Iterate grid columns
                for (int col = 0; col < grid.Columns.Count; col++)
                {
                    // Evaluate if there is a control in this cell
                    if (grid[col, row].Tag != null)
                    {
                        // Local reference
                        var cellInfo = (CellInfo)grid[col, row].Tag;
                        // Is there a match (if yes, it will be a label)
                        if (cellInfo.Name == name)
                        {
                            stillInUse = true;
                            break;
                        }
                        else
                        {
                            // Get control in this cell
                            var control = grid.Controls[cellInfo.Name];
                            // If a flowlayout panel, can interogate controls collection
                            if (control.GetType() == typeof(FlowLayoutPanel))
                            {
                                if (control.Controls.ContainsKey(name))
                                {
                                    stillInUse = true;
                                    break;
                                }
                            }
                            // It a tab, need to iterate tab pages and recursion
                            else if (control.GetType() == typeof(TabControl))
                            {
                                foreach (TabPage tabPage in ((TabControl)control).TabPages)
                                {
                                    stillInUse = IsStillInUse((DataGridView)tabPage.Controls[0], name);
                                    if (stillInUse)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    // Break early if found
                    if (stillInUse)
                    {
                        break;
                    }
                }
            }
            return stillInUse;
        }


        /// <summary>
        /// Delete children control
        /// </summary>
        /// <param name="control">Parent control</param>
        private void DeleteChildren(Control control)
        {
            // Delete children if any
            for (int i = control.Controls.Count - 1; i >= 0; i--)
            {
                var child = control.Controls[i];
                var type = child.GetType();
                if (type == typeof(TabPage))
                {
                    // Delete tab page
                    DeleteTab(child);
                }
                else if (type == typeof(DataGridView))
                {
                    child.DragEnter -= DragEnterHandler;
                    child.DragDrop -= DragDropHandler;
                    ((DataGridView)child).CellClick -= PaletteCellClickHandler;
                    DeleteControl(child);
                }
                else
                {
                    // Delete container, grid, or label
                    DeleteControl(child);
                }
            }
        }

        /// <summary>
        /// Remove from Layout
        /// </summary>
        /// <param name="control">Control to be removed from layout</param>
        private void RemoveFromLayout(Control control)
        {
            var parentControl = control.Parent;

            // Remove from cell, if applicable
            if (control.Tag != null)
            {
                CellInfo cellInfo = (CellInfo)control.Tag;

                if (cellInfo != null && cellInfo.Control != null)
                {
                    cellInfo.Control[cellInfo.ColIndex, cellInfo.RowIndex].Tag = null;
                }
            }

            parentControl.Controls.Remove(control);
        }

        /// <summary>
        /// Drag handler to handle the effect in a common handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DragEnterHandler(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        /// <summary>
        /// Load entities
        /// </summary>
        /// <param name="entities">List of entities being generated</param>
        public void LoadEntities(List<BusinessView> entities)
        {
            // Set to local instance
            _entities = entities;

            // Setup tree
            treeEntities.Nodes.Clear();
            var entitiesNode = new TreeNode(Resources.AvailableFields) { Name = NodeEntities };

            // Iterate each view in entities list
            foreach (var businessView in _entities)
            {
                var entityName = businessView.Properties[BusinessView.Constants.EntityName];

                // Create node for entity
                var entityNode = new TreeNode(entityName) { Name = entityName };

                // Iterate each field in view's fields
                foreach (var businessField in businessView.Fields)
                {
                    var name = entityName + "_" + businessField.Name;

                    // Add to the controls list
                    _controlsList.Add(name, new ControlInfo()
                    {
                        BusinessField = businessField
                    });

                    // Create a node and add to entity node
                    var node = new TreeNode(businessField.Name) { Name = name };
                    entityNode.Nodes.Add(node);

                }

                // Add entity node
                entitiesNode.Nodes.Add(entityNode);
            }

            // Add entities to tree
            treeEntities.Nodes.Add(entitiesNode);
            treeEntities.ExpandAll();

            AssignEvents();
            InitProperties(null);
        }

        /// <summary>
        /// Remove from layout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_controlType == ControlType.None)
            {
                // Nothing to delete
                return;
            }

            if (_controlType == ControlType.Tab)
            {
                // Delete tab page
                DeleteTab(_selectedControl);
            }
            else
            {
                // Delete container, grid, or label
                DeleteControl(_selectedControl);
            }

            // Clear properties display
            _controlType = ControlType.None;
            _selectedControl = null;
            InitProperties(null);
        }

        /// <summary>
        /// Add a page to the selected tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPage_Click(object sender, EventArgs e)
        {
            // Get tab control from tab page
            var parentControl = (TabControl)_selectedControl.Parent;

            var control = GetTabPage(parentControl);        

            // Add the handlers
            // AddHandlers(control, true);

            // Add the page to the control
            parentControl.TabPages.Add(control);

            // Create palette for this tab page
            CreatePalette(control);

            // Ensure new tab page is the active page
            parentControl.SelectTab(control); ;

            // Invoke the handler
            ClickHandler(control, null);
        }

        /// <summary>
        /// Text of control has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPropText_TextChanged(object sender, EventArgs e)
        {
            if (_selectedControl != null)
            {
                if (_controlType == ControlType.Grid)
                {
                   GetControlInfo(_selectedControl.Name).Text = txtPropText.Text;
                }
                else
                {
                    _selectedControl.Text = txtPropText.Text;
                }
            }
        }

        /// <summary>
        /// Ok button to generate XML Layout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            // Create XML for layout and close form
            XMLLayout = BuildXDocument();
            Close();
        }

        /// <summary>
        /// Cancel button to exit without generation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            XMLLayout = null;
            Close();
        }

        /// <summary> Build XML Document from controls </summary>
        private XDocument BuildXDocument()
        {
            // Document
            var xDocument = new XDocument();
            var layoutElement = new XElement(NodeLayout);
            var controlsElement = new XElement(NodeControls);

            // Build XML from controls  
            BuildXmlFromControls((DataGridView)splitDesigner.Panel1.Controls[PrefixPalette + "1"], controlsElement);

            // Add elements?
            if (controlsElement.HasElements)
            {
                layoutElement.Add(controlsElement);
                xDocument.Add(layoutElement);
                return xDocument;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Iterate palette for building XML
        /// </summary>
        /// <param name="grid">Grid/Palette to evaluate</param>
        /// <param name="element">XElement </param>
        private void BuildXmlFromControls(DataGridView grid, XElement element)
        {
            var key = string.Empty;

            // Iterate grid rows
            for (int row = 0; row < grid.Rows.Count; row++)
            {
                // Elements for row
                // Init
                XElement formGroupElement = null;
                XElement formGroupControlsElement = null;

                // Control found on a row will be in a new row
                var newRow = false;

                // Iterate grid columns
                for (int col = 0; col < grid.Columns.Count; col++)
                {
                    // Is there is a control in this cell
                    if (grid[col, row].Tag != null)
                    {
                        // A control was found. If first on row add div for new row
                        if (!newRow)
                        {
                            formGroupElement = new XElement(NodeControl);
                            formGroupElement.Add(new XAttribute(AttributeType, AttributeDiv));
                            formGroupElement.Add(new XAttribute(AttributeRewRow, AttributeTrue));
                            formGroupElement.Add(new XAttribute(AttributeWidget, ""));
                            formGroupControlsElement = new XElement(NodeControls);
                            newRow = true;
                        }

                        // Local reference to cell info in tag of cell
                        var cellInfo = (CellInfo)grid[col, row].Tag;
                        // Get control in this cell
                        var control = grid.Controls[cellInfo.Name];

                        // If a label (business view model property) then create an element (no children)
                        if (control.GetType() == typeof(Label))
                        {
                            // Create control element
                            var controlElement = new XElement(NodeControl);
                            var controlInfo = GetControlInfo(control.Name);

                            // Add attributes
                            controlElement.Add(new XAttribute(AttributeType, AttributeDiv));
                            controlElement.Add(new XAttribute(AttributeRewRow, AttributeFalse));
                            controlElement.Add(new XAttribute(AttributeWidget, controlInfo.Widget));
                            controlElement.Add(new XAttribute(AttributeEntity, controlInfo.ParentNodeName));
                            controlElement.Add(new XAttribute(AttributeProperty, control.Text));

                            // Add to controls element
                            formGroupControlsElement.Add(controlElement);

                            // If a drop down/date/checkbox/radio button widget, then add to the list of controls
                            key = string.Empty;
                            if (controlInfo.Widget.Equals(WidgetDropDown))
                            {
                                key = WidgetDropDown;
                            }
                            // If a date widget, then add to the list of date controls
                            else if (controlInfo.Widget.Equals(WidgetDateTime))
                            {
                                key = WidgetDateTime;
                            }
                            // If a checkbox widget, then add to the list of checkbox controls
                            else if (controlInfo.Widget.Equals(WidgetCheckbox))
                            {
                                key = WidgetCheckbox;
                            }
                            // If a radio buttons widget, then add to the list of radio buttons controls
                            else if (controlInfo.Widget.Equals(WidgetRadioButtons))
                            {
                                key = WidgetRadioButtons;
                            }
                            // Add to dictionary?
                            if (!string.IsNullOrEmpty(key))
                            {
                                if (Widgets.ContainsKey(key))
                                {
                                    Widgets[key].Add(control.Text);
                                }
                                else
                                {
                                    Widgets.Add(key, new List<string> { control.Text });
                                }
                            }
                        }

                        // If a tab, need to iterate tab pages and recursion
                        else if (control.GetType() == typeof(TabControl))
                        {
                            // Create control element
                            var controlElement = new XElement(NodeControl);

                            // Add attributes
                            controlElement.Add(new XAttribute(AttributeType, AttributeDiv));
                            controlElement.Add(new XAttribute(AttributeRewRow, AttributeFalse));
                            controlElement.Add(new XAttribute(AttributeWidget, WidgetTab));
                            controlElement.Add(new XAttribute(AttributeId, control.Name));

                            // Iterate tab pages
                            var tabPageControlsElement = new XElement(NodeControls);
                            foreach (TabPage tabPage in ((TabControl)control).TabPages)
                            {
                                // Controls and control element for tab page
                                var tabPageControlElement = new XElement(NodeControl);
                                var tabPageName = tabPage.Text.Replace(" ", string.Empty);

                                // Add attributes
                                tabPageControlElement.Add(new XAttribute(AttributeType, AttributeLi));
                                tabPageControlElement.Add(new XAttribute(AttributeRewRow, AttributeTrue));
                                tabPageControlElement.Add(new XAttribute(AttributeWidget, WidgetTabPage));
                                tabPageControlElement.Add(new XAttribute(AttributeId, tabPageName));
                                tabPageControlElement.Add(new XAttribute(AttributeText, tabPage.Text));

                                // Now, recursion with palette on tab page
                                BuildXmlFromControls((DataGridView)tabPage.Controls[0], tabPageControlElement);

                                // Add to controls element
                                tabPageControlsElement.Add(tabPageControlElement);

                                // Add to dictionary
                                key = WidgetTabPage;
                                if (Widgets.ContainsKey(key))
                                {
                                    Widgets[key].Add(tabPageName);
                                }
                                else
                                {
                                    Widgets.Add(key, new List<string> { tabPageName });
                                }

                            }

                            // Add to control and controls elements
                            controlElement.Add(tabPageControlsElement);
                            formGroupControlsElement.Add(controlElement);

                            // Add to dictionary
                            key = WidgetTab;
                            if (Widgets.ContainsKey(key))
                            {
                                Widgets[key].Add(control.Name);
                            }
                            else
                            {
                                Widgets.Add(key, new List<string> { control.Name });
                            }

                        }
                        // If a grid, need to iterate children
                        else if (control.GetType() == typeof(FlowLayoutPanel))
                        {
                            // Create control element
                            var controlElement = new XElement(NodeControl);

                            // Add attributes
                            controlElement.Add(new XAttribute(AttributeType, AttributeDiv));
                            controlElement.Add(new XAttribute(AttributeRewRow, AttributeFalse));
                            controlElement.Add(new XAttribute(AttributeWidget, WidgetGrid));

                            // Iterate children (labels are business fields)
                            var childControlsElement = new XElement(NodeControls);
                            foreach (Control child in control.Controls)
                            {
                                // Only process labels (business fields)
                                if (child.GetType() == typeof(Label))
                                {
                                    // Control element for child
                                    var childControlElement = new XElement(NodeControl);
                                    var controlInfo = GetControlInfo(child.Name);

                                    // Add attributes
                                    childControlElement.Add(new XAttribute(AttributeType, "")); // TODO
                                    childControlElement.Add(new XAttribute(AttributeRewRow, "")); // TODO
                                    childControlElement.Add(new XAttribute(AttributeWidget, controlInfo.Widget));
                                    childControlElement.Add(new XAttribute(AttributeEntity, controlInfo.ParentNodeName));
                                    childControlElement.Add(new XAttribute(AttributeProperty, child.Text));

                                    // Add to controls element
                                    childControlsElement.Add(childControlElement);
                                }
                            }

                            // Add to control and controls elements
                            controlElement.Add(childControlsElement);
                            formGroupControlsElement.Add(controlElement);
                        }
                    }
                }
                // End of columns. If control(s) were found in columns for this row, then add to elements
                if (newRow)
                {
                    // Add to control element
                    formGroupElement.Add(formGroupControlsElement);
                    // Add to element entered to this routine
                    element.Add(formGroupElement);
                }
            }
        }

        private void PaletteCellClickHandler(object sender, DataGridViewCellEventArgs e)
        {
            ((DataGridView)sender).ClearSelection();
            InitProperties(null);
        }

    }
}
