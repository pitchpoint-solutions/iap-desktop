﻿//
// Copyright 2019 Google LLC
//
// Licensed to the Apache Software Foundation (ASF) under one
// or more contributor license agreements.  See the NOTICE file
// distributed with this work for additional information
// regarding copyright ownership.  The ASF licenses this file
// to you under the Apache License, Version 2.0 (the
// "License"); you may not use this file except in compliance
// with the License.  You may obtain a copy of the License at
// 
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.
//

using Google.Apis.Auth.OAuth2;
using System;
using System.IO;

namespace Google.Solutions.Compute.Test.Env
{
    public static class Defaults
    {
        private const string CloudPlatformScope = "https://www.googleapis.com/auth/cloud-platform";

        public static readonly string ProjectId = Environment.GetEnvironmentVariable("GOOGLE_CLOUD_PROJECT");
        public static readonly string Zone = "us-central1-a";

        public static GoogleCredential GetCredential()
        {
            var keyFile = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
            if (keyFile != null && File.Exists(keyFile))
            {
                return GoogleCredential.FromFile(keyFile).CreateScoped(CloudPlatformScope);
            }
            else
            {
                return GoogleCredential.GetApplicationDefault();
            }
        }
    }
}
