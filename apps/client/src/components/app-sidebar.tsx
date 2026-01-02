'use client';

import * as React from 'react';

import { NavUser } from '@/components/nav-user';
import {
    Sidebar,
    SidebarContent,
    SidebarFooter,
    SidebarRail,
} from '@/components/ui/sidebar';
import { sidebarNavConfig as data } from '@/config/sidebar-nav-config';
import { useAppStore } from '@/store';
import { useShallow } from 'zustand/react/shallow';
import { CollapsibleNavGroup } from './collapsible-nav-group';
import { SimpleNavGroup } from './simple-nav-group';

export function AppSidebar({ ...props }: React.ComponentProps<typeof Sidebar>) {
    const { toggleSidebar } = useAppStore(
        useShallow((s) => ({
            toggleSidebar: s.toggleSidebarState,
        }))
    );

    return (
        <Sidebar collapsible="icon" {...props}>
            <SidebarContent className="pt-5">
                <SimpleNavGroup Workflows={data.dashboard} />
                <SimpleNavGroup Workflows={data.admin} title="Admin Controls" />
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
                <CollapsibleNavGroup items={data.collapsibleGroup} />
            </SidebarContent>
            <SidebarFooter>
                <NavUser />
            </SidebarFooter>
            <SidebarRail onClick={() => toggleSidebar()} />
        </Sidebar>
    );
}
