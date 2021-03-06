// Copyright 2007-2018 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the
// License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the
// specific language governing permissions and limitations under the License.
namespace MassTransit.Transports.Tests.Observers
{
    using System;
    using System.Threading.Tasks;


    public class PublishObserver :
        IPublishObserver
    {
        readonly TaskCompletionSource<PublishContext> _postSend;
        readonly TaskCompletionSource<PublishContext> _preSend;
        readonly TaskCompletionSource<PublishContext> _sendFaulted;

        public PublishObserver()
        {
            _sendFaulted = new TaskCompletionSource<PublishContext>();
            _preSend = new TaskCompletionSource<PublishContext>();
            _postSend = new TaskCompletionSource<PublishContext>();
        }

        public Task<PublishContext> PrePublished
        {
            get { return _preSend.Task; }
        }

        public Task<PublishContext> PostPublished
        {
            get { return _postSend.Task; }
        }

        public Task<PublishContext> PublishFaulted
        {
            get { return _sendFaulted.Task; }
        }

        public async Task PrePublish<T>(PublishContext<T> context)
            where T : class
        {
            _preSend.TrySetResult(context);
        }

        public async Task PostPublish<T>(PublishContext<T> context)
            where T : class
        {
            _postSend.TrySetResult(context);
        }

        public async Task PublishFault<T>(PublishContext<T> context, Exception exception)
            where T : class
        {
            _sendFaulted.TrySetResult(context);
        }
    }
}