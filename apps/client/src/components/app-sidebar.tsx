'use client';

import * as React from 'react';

import { useGetUser } from '@/api/user-api';
import { NavUser } from '@/components/nav-user';
import {
    Sidebar,
    SidebarContent,
    SidebarFooter,
    SidebarRail,
} from '@/components/ui/sidebar';
import { useAppStore } from '@/store';
import { useShallow } from 'zustand/react/shallow';
import { SimpleNavGroup } from './simple-nav-group';
import { sidebarNavConfig as data } from '@/config/sidebar-navs-config';
import { CollapsibleNavGroup } from './collapsible-nav-group';

export function AppSidebar({ ...props }: React.ComponentProps<typeof Sidebar>) {
    const { data: user } = useGetUser();
    const { toggleSidebar } = useAppStore(
        useShallow((s) => ({
            toggleSidebar: s.toggleSidebarState,
        }))
    );

    data.user.name = `${user?.firstName} ${user?.lastName}`;
    // eslint-disable-next-line
    data.user.email = user?.email! ?? 'unknown@example.com';

    return (
        <Sidebar collapsible="icon" {...props}>
            <SidebarContent className="pt-5">
                <SimpleNavGroup Workflows={data.dashboard} />
                <SimpleNavGroup
                    Workflows={data.admin}
                    title="Admin Controlls"
                />
                <SimpleNavGroup Workflows={data.myWork} title="My Work" />
                <SimpleNavGroup
                    Workflows={data.coreWorkflows}
                    title="Core Workflows"
                />
                <SimpleNavGroup
                    Workflows={data.supportingWorkflows}
                    title="Supporting Workflows"
                />
                <SimpleNavGroup Workflows={data.other} title="Other" />
                <CollapsibleNavGroup items={data.collapsibleGrop} />
            </SidebarContent>
            <SidebarFooter>
                <NavUser user={data.user} />
            </SidebarFooter>
            <SidebarRail onClick={() => toggleSidebar()} />
        </Sidebar>
    );
}
