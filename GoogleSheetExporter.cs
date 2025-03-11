using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.CompilerServices;

namespace chaeHa_Helper
{
    public class GoogleSheetExporter
    {
        public struct Data
        {
            public string googleSheetUrl;
            public string googldeSheetRange;
            public string tablePath;
            public string exportPath;
            public bool showDoneDialog;
            public Crypto crypto;
        }

        public static bool export(Data data)
        {
            var result = false;
            StreamWriter tableSw = new StreamWriter(data.tablePath + "/" + "ChartDataTable.txt");
            StreamWriter exportSw = new StreamWriter(data.exportPath + "/" + "ChartDataTable.txt");

            var url = data.googleSheetUrl + data.googldeSheetRange;

            LoadGoogleSheet(url, (resBool, json) =>
                {
                    if (resBool)
                    {
                        result = true;
                        if (!data.crypto.isEmpty())
                        {
                            tableSw.Write(chaeHa_AES.Encode(json, data.crypto));
                        }
                        else
                        {
                            tableSw.Write(json);
                        }

                        exportSw.Write(json.ToString());

                        tableSw.Close();
                        exportSw.Close();

                        AssetDatabase.Refresh();


                        Debug.Log("���۽�Ʈ �ͽ���Ʈ�� �Ϸ� �Ǿ����ϴ�.");

                        if (data.showDoneDialog)
                            EditorUtility.DisplayDialog("���۽�Ʈ �ͽ���Ʈ", " �ͽ���Ʈ�� �Ϸ� �Ǿ����ϴ�.", "Ȯ��");

                        result = true;
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("���� ��Ʈ �ͽ���Ʈ ���� ", "��Ʈ��ũ ����", "Ȯ��");
                        result = false;
                    }
                });

            return result;
        }

        private async static void LoadGoogleSheet(string address, Action<bool, string> callback)
        {
            UnityWebRequest link = UnityWebRequest.Get(address);
            await link.SendWebRequest();

            if (link.result == UnityWebRequest.Result.ConnectionError ||
                link.result == UnityWebRequest.Result.ProtocolError)
            {
                callback?.Invoke(false, null);
            }
            else
            {
                var json = link.downloadHandler.text;
                callback?.Invoke(true, json);
            }
        }
    }

    /// <summary>
    /// ����Ƽ 6 ���ĺ��� �Ʒ� ����� �⺻ �����ϴ� �� ����, ������ ����Ƽ 6���� �����̶�� �Ʒ� ����ü�� �����ؼ� ����ؾ� �Ѵ�.
    /// </summary>
    public struct UnityWebRequestAwaiter : INotifyCompletion
    {
        private UnityWebRequestAsyncOperation asyncOp;
        private Action continuation;

        public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation asyncOp)
        {
            this.asyncOp = asyncOp;
            continuation = null;
        }

        public bool IsCompleted { get { return asyncOp.isDone; } }

        public void GetResult() { }

        public void OnCompleted(Action continuation)
        {
            this.continuation = continuation;
            asyncOp.completed += OnRequestCompleted;
        }

        private void OnRequestCompleted(AsyncOperation obj)
        {
            continuation?.Invoke();
        }
    }

    public static class ExtensionMethods
    {
        public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp)
        {
            return new UnityWebRequestAwaiter(asyncOp);
        }
    }
}
