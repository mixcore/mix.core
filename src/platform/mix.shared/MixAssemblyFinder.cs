// ============================================
// Mixcore CMS
// Copyright (c) Mixcore Foundation. All rights reserved.
// Licensed under the GNU Affero General Public License v3.0 (AGPL-3.0).
// See LICENSE file in the project root for full license information.
// Commercial licenses available at https://mixcore.org/licensing
// ============================================

using System.Reflection;

namespace Mix.Shared
{
    public class MixAssemblyFinder
    {
        public static List<Assembly> GetAssembliesByPrefix(string prefix)
        {
            return AppDomain.CurrentDomain.GetAssemblies().Where(IsSelectedModule(prefix)).ToList();
        }

        private static Func<Assembly, bool> IsSelectedModule(string prefix)
        {
            return p => p.FullName.StartsWith(prefix);
        }
    }
}
