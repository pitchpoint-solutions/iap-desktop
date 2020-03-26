﻿using Google.Solutions.Compute.Iap;
using Google.Solutions.IapDesktop.Application.ObjectModel;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Google.Solutions.IapDesktop.Application.Services
{
    public interface ITunnelService
    {
        Task<Tunnel> CreateTunnelAsync(TunnelDestination tunnelEndpoint);
    }

    public class TunnelService : ITunnelService
    {
        private readonly IAuthorizationService authorizationService;

        public TunnelService(IServiceProvider serviceProvider)
        {
            this.authorizationService = serviceProvider.GetService<IAuthorizationService>();
        }

        public Task<Tunnel> CreateTunnelAsync(TunnelDestination tunnelEndpoint)
        {
            var iapEndpoint = new IapTunnelingEndpoint(
                this.authorizationService.Authorization.Credential,
                tunnelEndpoint.Instance,
                tunnelEndpoint.RemotePort,
                IapTunnelingEndpoint.DefaultNetworkInterface);


            // Start listener to enable clients to connect. Do not await
            // the listener as we want to continue listeining in the
            // background.
            var listener = SshRelayListener.CreateLocalListener(iapEndpoint);
            var cts = new CancellationTokenSource();

            _ = listener.ListenAsync(cts.Token);

            // Return the tunnel which allows the listener to be stopped
            // via the CancellationTokenSource.
            return Task.FromResult(new Tunnel(iapEndpoint, listener, cts));
        }
    }
}
