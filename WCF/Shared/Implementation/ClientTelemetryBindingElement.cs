﻿using System;
using System.ServiceModel.Channels;

namespace Microsoft.ApplicationInsights.Wcf.Implementation
{
    class ClientTelemetryBindingElement : BindingElement
    {
        private TelemetryClient telemetryClient;
        private Type contractType;
        private ClientOperationMap operationMap;

        public ClientTelemetryBindingElement(TelemetryClient client, Type contract, ClientOperationMap map)
        {
            if ( client == null )
            {
                throw new ArgumentNullException(nameof(client));
            }
            if ( contract == null )
            {
                throw new ArgumentNullException(nameof(contract));
            }
            if ( map == null )
            {
                throw new ArgumentNullException(nameof(map));
            }
            this.telemetryClient = client;
            this.contractType = contract;
            this.operationMap = map;
        }

        public override BindingElement Clone()
        {
            return new ClientTelemetryBindingElement(telemetryClient, contractType, operationMap);
        }

        public override T GetProperty<T>(BindingContext context)
        {
            return context.GetInnerProperty<T>();
        }


        public override bool CanBuildChannelFactory<TChannel>(BindingContext context)
        {
            if ( context == null )
            {
                throw new ArgumentNullException(nameof(context));
            }
            if ( IsSupportedChannelShape(typeof(TChannel)) )
            {
                return context.CanBuildInnerChannelFactory<TChannel>();
            }
            return false;
        }

        public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
        {
            if ( context == null )
            {
                throw new ArgumentNullException(nameof(context));
            }
            if ( !IsSupportedChannelShape(typeof(TChannel)) )
            {
                throw new InvalidOperationException("Unsupported channel shape: " + typeof(TChannel));
            }
            var innerFactory = context.BuildInnerChannelFactory<TChannel>();
            return new ClientTelemetryChannelFactory<TChannel>(innerFactory, telemetryClient, contractType, operationMap);
        }

        private bool IsSupportedChannelShape(Type type)
        {
            if ( type == typeof(IRequestChannel) )
            {
                return true;
            }
            return false;
        }

    }
}
