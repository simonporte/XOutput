﻿using System;
using System.Collections.Generic;
using System.Threading;
using XOutput.Api.Devices;
using XOutput.Core.DependencyInjection;

namespace XOutput.Server.Emulation.SCPToolkit
{
    public class ScpEmulator : IXboxEmulator
    {
        public bool Installed { get; private set; }

        public string Name => "SCPToolkit";

        public IEnumerable<DeviceTypes> SupportedDeviceTypes { get; } = new DeviceTypes[] { DeviceTypes.MicrosoftXbox360 };

        private int counter = 0;
        private ScpClient client;

        [ResolverMethod]
        public ScpEmulator()
        {
            Installed = Initialize();
        }

        public XboxDevice CreateXboxDevice()
        {
            int controllerIndex = Interlocked.Increment(ref counter);
            return new ScpDevice(controllerIndex, client);
        }

        public void Close()
        {
            Installed = false;
            client.UnplugAll();
            client.Dispose();
        }

        private bool Initialize()
        {
            try
            {
                client = new ScpClient();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
