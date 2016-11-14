﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UI;
using System.Collections.Generic;
using System;

namespace UnityClusterPackage {
	public abstract class UcpGuiAction : MonoBehaviour
	{
		public abstract void NodeSetUp();
	}

	public class UcpGui : MonoBehaviour {
		public UcpGuiAction DoAfterSetUp;
		public event Action<Node> NodeSetUp;

		private Node m_Node;
		private string m_ServerPort;
		private StringNodePoint m_Pa;
		private StringNodePoint m_Pb;
		private StringNodePoint m_Pc;
		private StringNodePoint m_Pe;

		private string m_ErrorMessages = string.Empty;

		protected static readonly Rect MainAreaFrame = new Rect(
			Screen.width / 2 / 2,
			(int) (Screen.height / 1.5 / 2),
			Screen.width / 2,
			(int)(Screen.height / 1.5)
		);

		// Use this for initialization
		protected virtual void Start () {
			m_Node = Node.CurrentNode;

			m_ServerPort = m_Node.NodeServer.Port.ToString();
			m_Pa = ConvertNodePoint(m_Node.NodeScreen.Pa);
			m_Pb = ConvertNodePoint(m_Node.NodeScreen.Pb);
			m_Pc = ConvertNodePoint(m_Node.NodeScreen.Pc);
			m_Pe = ConvertNodePoint(m_Node.NodeScreen.Pe);
		}

		protected void OnGUI() {
			GUILayout.BeginArea(MainAreaFrame); {
				GUILayout.Label("Node settings");

				// General stuff
				GUILayout.BeginHorizontal(); {
					// Id
					GUILayout.BeginVertical(MakeWidthOption(.33f)); {
						GUILayout.Label("Node Id");
						var idString = GUILayout.TextField(m_Node.Id.ToString());
						try {
							m_Node.Id = int.Parse(idString);
						} catch(Exception) { }
					} GUILayout.EndVertical();

					// Name
					GUILayout.BeginVertical(MakeWidthOption(.33f)); {
						GUILayout.Label("Node name");
						m_Node.Name = GUILayout.TextField(m_Node.Name);
					} GUILayout.EndVertical();

					// Nodes
					GUILayout.BeginVertical(MakeWidthOption(.32f)); {
						GUILayout.Label("Number of nodes");
						var nodes = GUILayout.TextField(m_Node.Nodes.ToString());
						try {
							m_Node.Nodes = int.Parse(nodes);
						} catch(Exception) { }
					} GUILayout.EndVertical();
				} GUILayout.EndHorizontal();

				// Server Data
				GUILayout.BeginHorizontal(); {
					// Is master node?
					m_Node.NodeType = GUILayout.Toggle(
						m_Node.NodeType == Node.Type.master,
						new GUIContent("Is master node", "Indicantes whether this instance is the master node"),
						MakeWidthOption(.33f)
					) ? Node.Type.master : Node.Type.slave;

					// Server Address
					GUILayout.BeginVertical(MakeWidthOption(.33f)); {
						GUILayout.Label("Server IP");
						m_Node.NodeServer.Ip = GUILayout.TextField(m_Node.NodeServer.Ip);
					} GUILayout.EndVertical();

					// Server Port
					GUILayout.BeginVertical(MakeWidthOption(.32f)); {
						GUILayout.Label("Server Port");
						m_ServerPort = GUILayout.TextField(m_ServerPort);
					} GUILayout.EndVertical();
				}; GUILayout.EndHorizontal();

				// Screen data
				GUILayout.BeginHorizontal(); {
					// Is Stereo?
					m_Node.NodeScreen.Stereo = GUILayout.Toggle(
						m_Node.NodeScreen.Stereo,
						"Stereo",
						MakeWidthOption(.33f)
					);

					// Which eye stereo uses
					m_Node.NodeScreen.ScreenEye = GUILayout.Toggle(
						m_Node.NodeScreen.ScreenEye == Node.Eye.right,
						"Right eye?",
						MakeWidthOption(.5f)
					) ? Node.Eye.right : Node.Eye.left;

					// Whether it is using Google VR
					m_Node.NodeScreen.UsesGoogleVr = GUILayout.Toggle(
						m_Node.NodeScreen.UsesGoogleVr,
						"Google VR",
						MakeWidthOption(.33f)
					);
				} GUILayout.EndHorizontal();

				// Points
				DrawPoints("Pa", m_Pa);
				DrawPoints("Pb", m_Pb);
				DrawPoints("Pc", m_Pc);
				DrawPoints("Pe", m_Pe);

				// Buttons
				if (GUILayout.Button("Confirm")) {
					CheckAndSave();
				}
				if (GUILayout.Button("Confirm and save node-config.xml")) {
					CheckAndSave(() => {
						Node.SetNodeDataUp(m_Node);
					});
				}

				GuiCustomizationHook();

				// Error messages
				GUILayout.Label(m_ErrorMessages);

			} GUILayout.EndArea();
		}

		protected virtual void GuiCustomizationHook() { }

		private static void DrawPoints(string label, StringNodePoint point)
		{
			GUILayout.BeginHorizontal(); {
				point.X = DrawPoint(label + " X", point.X);
				point.Y = DrawPoint(label + " Y", point.Y);
				point.Z = DrawPoint(label + " Z", point.Z);
			} GUILayout.EndHorizontal();
		}

		private static string DrawPoint(string label, string pointValue) {
			GUILayout.BeginVertical(MakeWidthOption(.33f)); {
				GUILayout.Label(label);
				pointValue = GUILayout.TextField(pointValue);
			} GUILayout.EndVertical();

			return pointValue;
		}

		private void CheckAndSave(Action action = null)
		{
			var msgs = new System.Text.StringBuilder();
			bool valid = true;

			int port = 0;
			if (!int.TryParse(m_ServerPort, out port)) {
				msgs.AppendLine("INVALID SERVER PORT");
				valid = false;
			} else
				m_Node.NodeServer.Port = port;

			if (!ValidatePoint(m_Pa, m_Node.NodeScreen.Pa)) {
				msgs.AppendLine("INVALID PA COORDINATES");
				valid = false;
			}

			if (!ValidatePoint(m_Pb, m_Node.NodeScreen.Pb)) {
				msgs.AppendLine("INVALID PB COORDINATES");
				valid = false;
			}

			if (!ValidatePoint(m_Pc, m_Node.NodeScreen.Pc)) {
				msgs.AppendLine("INVALID PC COORDINATES");
				valid = false;
			}

			if (!ValidatePoint(m_Pe, m_Node.NodeScreen.Pe)) {
				msgs.AppendLine("INVALID PE COORDINATES");
				valid = false;
			}

			if (valid) {
				if (action != null)
					action();

				if (DoAfterSetUp != null) {
					DoAfterSetUp.NodeSetUp();
				}

				if (NodeSetUp != null) {
					NodeSetUp(m_Node);
				}
			} else {
				m_ErrorMessages = msgs.ToString();
			}
		}

		private static StringNodePoint ConvertNodePoint(Node.Point point) {
			return new StringNodePoint() {
				X = point.X.ToString(),
				Y = point.Y.ToString(),
				Z = point.Z.ToString()
			};
		}

		private static bool ValidatePoint(StringNodePoint stringPoint, Node.Point point)
		{
			float value;

			if (float.TryParse(stringPoint.X, out value))
				point.X = value;
			else
				return false;

			if (float.TryParse(stringPoint.Y, out value))
				point.Y = value;
			else
				return false;

			if (float.TryParse(stringPoint.Z, out value))
				point.Z = value;
			else
				return false;

			return true;
		}

		protected static GUILayoutOption MakeWidthOption(float portionOfArea) {
			return GUILayout.Width((int)(MainAreaFrame.width * portionOfArea));
		}

		[System.Serializable]
		public class StringNodePoint
		{
			public string X {
				get;
				set;
			}
			public string Y {
				get;
				set;
			}
			public string Z {
				get;
				set;
			}
		}
	}
}
