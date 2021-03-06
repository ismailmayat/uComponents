﻿using System.Linq;
using System.Xml;
using System.Xml.XPath;
using uComponents.Core;
using umbraco;

namespace uComponents.XsltExtensions
{
	/// <summary>
	/// The Nodes class exposes XSLT extensions to access nodes from Umbraco.
	/// </summary>
	[XsltExtension("ucomponents.nodes")]
	public class Nodes
	{
		/// <summary>
		/// Gets the expiry date of a node.
		/// </summary>
		/// <param name="nodeId">The node id.</param>
		/// <returns>Returns the expiry date of a node.</returns>
		public static string GetExpireDate(int nodeId)
		{
			var document = uQuery.GetDocument(nodeId);

			if (document != null)
			{
				return document.ExpireDate.ToString();
			}

			return string.Empty;
		}

		/// <summary>
		/// Gets the node Id by URL.
		/// </summary>
		/// <param name="url">The URL to get the XML node from.</param>
		/// <returns>Returns the node Id.</returns>
		public static int GetNodeIdByUrl(string url)
		{
			return uQuery.GetNodeIdByUrl(url);
		}

		/// <summary>
		/// Gets the node id by path level.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="level">The level.</param>
		/// <returns>Returns the node id for a given path level.</returns>
		public static string GetNodeIdByPathLevel(string path, int level)
		{
			var nodeIds = path.Split(Constants.Common.COMMA).ToList();

			if (nodeIds.Count > level)
			{
				return nodeIds[level];
			}

			return uQuery.RootNodeId.ToString();
		}

		/// <summary>
		/// Gets the release date of a node.
		/// </summary>
		/// <param name="nodeId">The node id.</param>
		/// <returns>Returns the release date of a node.</returns>
		public static string GetReleaseDate(int nodeId)
		{
			var document = uQuery.GetDocument(nodeId);

			if (document != null)
			{
				return document.ReleaseDate.ToString();
			}

			return string.Empty;
		}

		/// <summary>
		/// Gets the unique id of a node.
		/// </summary>
		/// <param name="nodeId">The node id.</param>
		/// <returns>Returns the unique id of a node.</returns>
		public static string GetUniqueId(int nodeId)
		{
			return Cms.GetUniqueId(nodeId);
		}

		/// <summary>
		/// Gets a list of XML nodes by CSV.
		/// </summary>
		/// <param name="csv">The CSV list.</param>
		/// <returns>
		/// Returns an XPathNodeIterator of the nodes from the CSV list.
		/// </returns>
		public static XPathNodeIterator GetXmlNodeByCsv(string csv)
		{
			var ids = uQuery.GetCsvIds(csv);
			var xpath = string.Concat("descendant::*[@id='", string.Join("' or @id='", ids.ToArray()), "']");
			return library.GetXmlNodeByXPath(xpath);
		}

		/// <summary>
		/// Gets a list of XML nodes by CSV (with an option to persist the order).
		/// </summary>
		/// <param name="csv">The CSV of node IDs.</param>
		/// <param name="persistOrder">if set to <c>true</c> [persist order].</param>
		/// <returns>
		/// Returns an XPathNodeIterator of the nodes from the CSV list.
		/// </returns>
		public static XPathNodeIterator GetXmlNodeByCsv(string csv, bool persistOrder)
		{
			if (!persistOrder)
			{
				return GetXmlNodeByCsv(csv);
			}

			var xd = new XmlDocument();
			xd.LoadXml("<root/>");

			var nodes = uQuery.GetNodesByCsv(csv);

			foreach (var node in nodes)
			{
				var xn = xd.ImportNode(node.ToXml(), true);
				xd.DocumentElement.AppendChild(xn);
			}

			return xd.CreateNavigator().Select("/root/child::*[@isDoc]");
		}

		// TODO: [LK] Add overload for GetXmlNodeByCsv to accept an XPath expression

		/// <summary>
		/// Gets the XML node by path level.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="level">The level.</param>
		/// <returns>Returns an XML node by path level.</returns>
		public static XPathNodeIterator GetXmlNodeByPathLevel(string path, int level)
		{
			var nodeId = GetNodeIdByPathLevel(path, level);
			return library.GetXmlNodeById(nodeId);
		}

		/// <summary>
		/// Gets the XML node by URL.
		/// </summary>
		/// <param name="url">The URL to get the XML node from.</param>
		/// <returns>Returns the XML for the node.</returns>
		public static XPathNodeIterator GetXmlNodeByUrl(string url)
		{
			var nodeId = uQuery.GetNodeIdByUrl(url);
			return library.GetXmlNodeById(nodeId.ToString());
		}
	}
}