modules.component('mainSideBar', {
    templateUrl: '/app/app-portal/components/main-side-bar/main-side-bar.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings', 'TranslatorService', function ($rootScope, $scope, ngAppSettings, translatorService) {
        var ctrl = this;
        ctrl.init = async function () {
            ctrl.items = [
                {
                    title: 'portal_dashboard',
                    shortTitle: 'portal_short_dashboard',
                    icon: 'mi mi-Tiles',
                    href: '/portal',
                    subMenus: []
                },
                {
                    title: 'portal_articles',
                    shortTitle: ('portal_articles'),
                    icon: 'mi mi-ReadingList',
                    href: '#',
                    subMenus: [
                        {
                            title: ('portal_create'),
                            href: '/portal/article/create',
                            icon: 'mi mi-Add'
                        },
                        {
                            title: 'portal_list',
                            href: '/portal/article/list',
                            icon: 'mi mi-List'
                        }
                    ]
                },
                // {
                //     title: 'portal_products',
                //     shortTitle: 'portal_products',
                //     icon: 'mi mi-Package',
                //     href: '#',
                //     subMenus: [
                //         {
                //             title: ('portal_create'),
                //             href: '/portal/product/create',
                //             icon: 'mi mi-Add'
                //         },
                //         {
                //             title: 'portal_list',
                //             href: '/portal/product/list',
                //             icon: 'mi mi-List'
                //         }
                //     ]
                // },
                // {
                //     title: 'portal_orders',
                //     shortTitle: 'portal_orders',
                //     icon: 'mi mi-CashDrawer',
                //     href: '#',
                //     subMenus: [
                //         {
                //             title: ('portal_create'),
                //             href: '/portal/order/create',
                //             icon: 'mi mi-Add'
                //         },
                //         {
                //             title: 'portal_list',
                //             href: '/portal/order/list',
                //             icon: 'mi mi-List'
                //         }
                //     ]
                // },
                {
                    title: 'portal_pages',
                    shortTitle: 'portal_pages',
                    icon: 'mi mi-Page',
                    href: '#',
                    subMenus: [
                        {
                            title: ('portal_create'),
                            href: '/portal/page/create',
                            icon: 'mi mi-Add'
                        },
                        {
                            title: 'portal_list',
                            href: '/portal/page/list',
                            icon: 'mi mi-List'
                        }
                    ]
                },
                {
                    title: 'portal_modules',
                    shortTitle: 'portal_modules',
                    icon: 'mi mi-ResolutionLegacy',
                    href: '#',
                    subMenus: [
                        {
                            title: ('portal_create'),
                            href: '/portal/module/create',
                            icon: 'mi mi-Add'
                        },
                        {
                            title: 'portal_list',
                            href: '/portal/module/list',
                            icon: 'mi mi-List'
                        }
                    ]
                },
                {
                    title: 'portal_themes',
                    shortTitle: 'portal_themes',
                    icon: 'mi mi-Personalize',
                    href: '#',
                    subMenus: [
                        {
                            title: ('portal_create'),
                            href: '/portal/theme/create',
                            icon: 'mi mi-Add'
                        },
                        {
                            title: 'portal_list',
                            href: '/portal/theme/list',
                            icon: 'mi mi-List'
                        }
                    ]
                },
                {
                    title: 'portal_media',
                    shortTitle: 'Media',
                    icon: 'mi mi-Photo2',
                    href: '#',
                    subMenus: [
                        {
                            title: ('portal_create'),
                            href: '/portal/media/create',
                            icon: 'mi mi-Add'
                        },
                        {
                            title: 'portal_list',
                            href: '/portal/media/list',
                            icon: 'mi mi-List'
                        }
                    ]
                },
                {
                    title: 'portal_file',
                    shortTitle: 'File',
                    icon: 'mi mi-FileExplorer',
                    href: '#',
                    subMenus: [
                        {
                            title: ('portal_create'),
                            href: '/portal/file/create',
                            icon: 'mi mi-Add'
                        },
                        {
                            title: 'portal_list',
                            href: '/portal/file/list',
                            icon: 'mi mi-List'
                        }
                    ]
                },
                {
                    title: 'portal_users',
                    shortTitle: 'Users',
                    icon: 'mi mi-Contact',
                    href: '#',
                    subMenus: [
                        {
                            title: ('portal_create'),
                            href: '/portal/user/create',
                            icon: 'mi mi-Add'
                        },
                        {
                            title: 'portal_list',
                            href: '/portal/user/list',
                            icon: 'mi mi-List'
                        },
                        {
                            title: 'Roles',
                            href: '/portal/role/list',
                            icon: 'mi mi-Permissions'
                        }
                    ]
                },
                {
                    title: 'portal_settings',
                    shortTitle: 'Settings',
                    icon: 'mi mi-Settings mi-spin',
                    href: '#',
                    subMenus: [
                        {
                            title: 'portal_app_settings',
                            href: '/portal/app-settings/details',
                            icon: 'mi mi-ViewAll'
                        },
                        {
                            title: ('portal_create'),
                            href: '/portal/configuration/create',
                            icon: 'mi mi-Add'
                        },
                        {
                            title: 'portal_list',
                            href: '/portal/configuration/list',
                            icon: 'mi mi-List'
                        }
                    ]
                },
                {
                    title: 'portal_import',
                    shortTitle: 'portal_short_import',
                    icon: 'mi mi-Upload',
                    href: '/portal/import',
                    subMenus: []
                },
                {
                    title: 'language',
                    shortTitle: 'Language',
                    icon: 'mi mi-TimeLanguage',
                    href: '#',
                    subMenus: [
                        {
                            title: 'portal_create',
                            href: '/portal/language/create',
                            icon: 'mi mi-Add'
                        },
                        {
                            title: 'List',
                            href: '/portal/language/list',
                            icon: 'mi mi-List'
                        }
                    ]
                },
                {
                    title: 'portal_cultures',
                    shortTitle: 'portal_short_cultures',
                    icon: 'mi mi-Globe mi-spin',
                    href: '#',
                    subMenus: [
                        {
                            title: ('portal_create'),
                            href: '/portal/culture/create',
                            icon: 'mi mi-Add'
                        },
                        {
                            title: 'portal_list',
                            href: '/portal/culture/list',
                            icon: 'mi mi-List'
                        }
                    ]
                },
                {
                    title: 'portal_permissions',
                    shortTitle: 'portal_short_portal_permissions',
                    icon: 'mi mi-LockscreenDesktop',
                    href: '#',
                    subMenus: [
                        {
                            title: ('portal_create'),
                            href: '/portal/permission/create',
                            icon: 'mi mi-Add'
                        },
                        {
                            title: 'portal_list',
                            href: '/portal/permission/list',
                            icon: 'mi mi-List'
                        }
                    ]
                }
            ];
        };
    }],
    bindings: {
    }
});