using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Diagnostics;


namespace chaeHa_Helper
{
    public class GoogleSheetExporterWindow : EditorWindow
    {
        private string m_version = "0.0.1";
        private string m_exportPath = "";
        private string m_excelPath = "";
        private string m_googldeSheetUrlPath = "";
        private string m_googldeSheetRange = "";
        private GUIStyle m_horizontalLine = null;
        private bool m_isInitialized = false;

        [MenuItem("Table/Export")]
        static void Init()
        {
            GoogleSheetExporterWindow window = (GoogleSheetExporterWindow)EditorWindow.GetWindow(typeof(GoogleSheetExporterWindow));
            window.Show();
            window.initialize();
        }

        public void initialize()
        {
            m_isInitialized = true;

            loadPath();
            initHorizontalLine();
        }

        private void initHorizontalLine()
        {
            m_horizontalLine = new GUIStyle();
            m_horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
            m_horizontalLine.margin = new RectOffset(0, 0, 4, 4);
            m_horizontalLine.fixedHeight = 1;
        }

        private void OnGUI()
        {
            if (!m_isInitialized)
                initialize();

            onVersion();
            onExcelPath();
            onGoogleSheetPath();
            onGoogleSheetRangePath();
            onExportPath();
            onExport();
        }

        private void onVersion()
        {
            GUILayout.Label("Ver " + m_version, EditorStyles.boldLabel);
        }

        private void onGoogleSheetPath()
        {
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            EditorGUIUtility.labelWidth = 70.0f;
            m_googldeSheetUrlPath = EditorGUILayout.TextField("Google URL", m_googldeSheetUrlPath);
            savePath();

            EditorGUILayout.EndHorizontal();
        }

        private void onGoogleSheetRangePath()
        {
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            EditorGUIUtility.labelWidth = 70.0f;

            m_googldeSheetRange = EditorGUILayout.TextField("Google Sheet Range", m_googldeSheetRange);
            savePath();

            EditorGUILayout.EndHorizontal();
        }

        private void onExportPath()
        {
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            EditorGUIUtility.labelWidth = 70.0f;
            EditorGUILayout.LabelField("ExportPath", m_exportPath);

            if (GUILayout.Button("...", GUILayout.MaxWidth(30.0f)))
            {
                m_exportPath = EditorUtility.OpenFolderPanel("Select Path", "", "");
                savePath();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void onExcelPath()
        {
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            EditorGUIUtility.labelWidth = 70.0f;
            EditorGUILayout.LabelField("ExcelPath", m_excelPath);

            if (GUILayout.Button("...", GUILayout.MaxWidth(30.0f)))
            {
                m_excelPath = EditorUtility.OpenFolderPanel("Select Path", "", "");
                savePath();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void savePath()
        {
            string tableInfoPath = getTableInfoPath();
            StreamWriter writer = new StreamWriter(tableInfoPath);

            writer.WriteLine(m_excelPath);
            writer.WriteLine(m_googldeSheetUrlPath);
            writer.WriteLine(m_googldeSheetRange);
            writer.WriteLine(m_exportPath);

            writer.Close();
        }

        private string getTableInfoPath()
        {
            return Application.dataPath + "/../TableExcelPath.txt";
        }

        private void onExport()
        {
            if (null == m_googldeSheetUrlPath)
                return;
            else if (null != m_googldeSheetRange)
            {
                var oriText = m_googldeSheetUrlPath;
                string ReplaceResult = oriText.Replace("/edit?usp=sharing", "/");
                m_googldeSheetUrlPath = ReplaceResult;
            }

            EditorGUILayout.Space();
            horizontalLine(Color.gray);

            onExportOne();
        }

        private void onExportOne()
        {
            EditorGUILayout.Space();

            Color oldColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Export"))
            {
                Crypto crypto = readJson<Crypto>("TableCrypto.txt");

                var exportData = new GoogleSheetExporter.Data
                {
                    googleSheetUrl = m_googldeSheetUrlPath,
                    googldeSheetRange = "export?format=tsv&range=" + m_googldeSheetRange,
                    tablePath = m_exportPath,
                    exportPath = m_excelPath,
                    showDoneDialog = false,
                    crypto = crypto,
                };
                GoogleSheetExporter.export(exportData);

                EditorUtility.DisplayDialog("구글 시트 익스포트", " 익스포트가 완료 되었습니다.", "확인");
            }
            GUI.backgroundColor = oldColor;
        }

        private void loadPath()
        {
            string tableInfoPath = getTableInfoPath();
            if (File.Exists(tableInfoPath))
            {
                using (StreamReader reader = new StreamReader(tableInfoPath))
                {
                    m_excelPath = reader.ReadLine();
                    m_googldeSheetUrlPath = reader.ReadLine();
                    m_googldeSheetRange = reader.ReadLine();
                    m_exportPath = reader.ReadLine();
                }
            }
        }
        private void horizontalLine(Color color)
        {
            var c = GUI.color;
            GUI.color = color;
            GUILayout.Box(GUIContent.none, m_horizontalLine);
            GUI.color = c;
        }

        public static T readJson<T>(string path)
        {
            string json = readStream(path);
            return JsonUtility.FromJson<T>(json);
        }


        public static string readStream(string path)
        {
            try
            {
                string text = null;
                using (StreamReader reader = new StreamReader(path))
                {
                    text = reader.ReadToEnd();
                }
                return text;
            }
            catch (Exception e)
            {
                return null;
            }            
        }
    }
}
