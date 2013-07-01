﻿namespace uComponents.DataTypes.DataTypeGrid.Factories
{
    using System;
    using System.Web.UI;

    using uComponents.DataTypes.DataTypeGrid.Model;

    using umbraco.editorControls.pagepicker;
    using umbraco.NodeFactory;
    using umbraco.interfaces;

    /// <summary>a
    /// Factory for the <see cref="PagePickerDataTypeFactory"/>
    /// </summary>
    [DataTypeFactory(Priority = -1)]
    public class PagePickerDataTypeFactory : BaseDataTypeFactory<PagePickerDataType>
    {
        /// <summary>
        /// Method for customizing the way the <paramref name="dataType" /> value is displayed in the grid.
        /// </summary>
        /// <remarks>Called when the grid displays the cell value for the specified <paramref name="dataType" />.</remarks>
        /// <param name="dataType">The <paramref name="dataType" /> instance.</param>
        /// <returns>The display value.</returns>
        public override string GetDisplayValue(PagePickerDataType dataType)
        {
            if (dataType.Data.Value != null)
            {
                int id;

                if (int.TryParse(dataType.Data.Value.ToString(), out id))
                {
                    try 
                    { 
                        var node = new Node(id);

                        return string.Format("<a href='editContent.aspx?id={0}' title='Edit content'>{1}</a>", node.Id, node.Name);
                    }
                    catch (Exception ex)
                    {
                        return string.Format("<span style='color: red;'>{0}</span>", ex.Message);
                    }
                }

                return dataType.Data.Value.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Method for getting the backing object for the specified <paramref name="dataType" />.
        /// </summary>
        /// <remarks>Called when the method <see cref="GridCell.GetObject{T}()"/> method is called on a <see cref="GridCell"/>.</remarks>
        /// <param name="dataType">The <paramref name="dataType" /> instance.</param>
        /// <returns>The backing object.</returns>
        public override object GetObject(PagePickerDataType dataType)
        {
            if (dataType.Data.Value != null)
            {
                int id;

                if (int.TryParse(dataType.Data.Value.ToString(), out id))
                {
                    return new Node(id);
                }
            }

            return default(Node);
        }

        /// <summary>
        /// Method for getting the control to use when validating the specified <see cref="IDataType" />.
        /// </summary>
        /// <param name="dataType">The <see cref="IDataType" /> instance.</param>
        /// <param name="editorControl">The <see cref="IDataType" /> editor control.</param>
        /// <returns>The control to validate.</returns>
        public override Control GetControlToValidate(PagePickerDataType dataType, Control editorControl)
        {
            var value = editorControl.Controls[0];

            return value;
        }
    }
}