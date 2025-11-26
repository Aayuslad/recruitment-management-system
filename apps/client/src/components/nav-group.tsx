'use client';

import { type LucideIcon } from 'lucide-react';

import {
    SidebarGroup,
    SidebarGroupLabel,
    SidebarMenu,
    SidebarMenuButton,
    SidebarMenuItem,
} from '@/components/ui/sidebar';
import { Link } from 'react-router-dom';

export function NavGroup({
    Workflows,
    title,
}: {
    Workflows: {
        name: string;
        url: string;
        icon: LucideIcon;
    }[];
    title?: string;
}) {
    return (
        <SidebarGroup>
            {title !== null && <SidebarGroupLabel>{title}</SidebarGroupLabel>}
            <SidebarMenu>
                {Workflows.map((item) => (
                    <SidebarMenuItem key={item.name}>
                        <SidebarMenuButton asChild tooltip={item.name}>
                            <Link to={item.url}>
                                <item.icon />
                                <span>{item.name}</span>
                            </Link>
                        </SidebarMenuButton>
                    </SidebarMenuItem>
                ))}
            </SidebarMenu>
        </SidebarGroup>
    );
}
