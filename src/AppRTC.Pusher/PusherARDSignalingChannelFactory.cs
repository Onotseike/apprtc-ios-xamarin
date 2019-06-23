﻿//
// PusherARDSignalingChannelFactory.cs
//
// Author:
//       valentingrigorean <valentin.grigorean1@gmail.com>
//
// Copyright (c) 2019 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Threading;

namespace AppRTC.Pusher
{
    public class PusherARDSignalingChannelFactory : IARDSignalingChannelFactory
    {
        private readonly PusherARDRoomServerClient _pusherARDRoomServerClient;
        private readonly bool _isLoopback;
        private int _count;

        public PusherARDSignalingChannelFactory(PusherARDRoomServerClient pusherARDRoomServerClient, bool isLoopback)
        {
            _pusherARDRoomServerClient = pusherARDRoomServerClient;
            _isLoopback = isLoopback;
        }


        public ARDSignalingChannel CreateChannel(string url, string restUrl, IARDSignalingChannelDelegate @delegate)
        {
            return new PusherARDSignalingChannel(url, restUrl, @delegate, PusherClientConfig.Default)
            {
                OnRegister = OnRegister,
                Client = _pusherARDRoomServerClient
            };
        }

        public ARDSignalingChannel CreateChannelLoopback(string url, string restUrl)
        {
            return new PusherLoopbackARDSignalingChannel(url, restUrl, PusherClientConfig.Default)
            {
                OnRegister = OnRegister,
                Client = _pusherARDRoomServerClient

            };
        }


        private void OnRegister()
        {
            Interlocked.Increment(ref _count);
            if (_isLoopback && _count == 2)
            {
                _pusherARDRoomServerClient.SetConnected();
            }
            else if (!_isLoopback)
            {
                _pusherARDRoomServerClient.SetConnected();
            }
        }
    }
}
