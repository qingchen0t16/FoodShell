using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ToastIO.API;
using ToastIO.Enum;
using ToastIO.Model;
using ToastIO.Package;
using UnityEngine;

namespace Assets.Scripts.Client
{
    public class FSClient
    {
        public static FSClient Instance = new FSClient();
        public FSRequest FSR;

        public FSClient()
        {
            FSR = new FSRequest();

            // 增加数据进入异常监听
            FSR.DataEnterExListener += (request, e) =>
            {
                Debug.Log($"{request.IP}:{request.Port} : 引发异常 > {e.Message}");
            };
            // 增加数据包解析异常监听
            FSR.PagePushFailedListener += (request, buff) =>
            {
                Debug.Log($"{request.IP}:{request.Port} : 数据包解析出现问题: " + $"数据长度:{buff.Length}");
            };
            // 增加请求关闭监听
            FSR.RequestCloseListener += (request, str) =>
            {
                Debug.Log($"{str}");
                return;
            };

            // 连接失败
            FSR.ConnectionFailedListener += (request, e) => {
                Debug.Log($"无法连接至服务器");
                return;
            };

            // 增加包体接收完毕监听
            FSR.EndReceiveListener += EndReceive;
        }

        /// <summary>
        /// 启动服务器
        /// </summary>
        public void Open(string host,int port)
        {
            FSR.Bind(host, port);
        }

        /// <summary>
        /// 数据到达
        /// </summary>
        /// <param name="sp"></param>
        public void EndReceive(SourcePackage sp)
        {
            EndPoint? point = sp.RequestSocket.LocalEndPoint;
            Debug.Log($"{(point is null ? "未知客户" : (IPEndPoint)point)} : 请求 > {sp.Header}");

            switch (sp.SendType)
            {
                case SendType.Text:
                    break;
                case SendType.Object:
                    if (sp.Header == "Message")
                    {
                        Debug.Log(sp.GetSource<SendMessage>().Message);
                    }
                    break;
            }
        }
    }
}
