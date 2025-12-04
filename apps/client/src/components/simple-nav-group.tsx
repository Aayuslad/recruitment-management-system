import { type LucideIcon } from 'lucide-react';

import {
    SidebarGroup,
    SidebarGroupLabel,
    SidebarMenu,
    SidebarMenuButton,
    SidebarMenuItem,
} from '@/components/ui/sidebar';
import { NavLink } from 'react-router-dom';

export function SimpleNavGroup({
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
            <SidebarMenu className="font-semibold">
                {Workflows.map((item) => (
                    <NavLink to={item.url} key={item.name}>
                        {({ isActive }) => (
                            <SidebarMenuItem>
                                <SidebarMenuButton
                                    tooltip={item.name}
                                    className={isActive ? 'bg-accent' : ''}
                                >
                                    <item.icon />
                                    <span>{item.name}</span>
                                </SidebarMenuButton>
                            </SidebarMenuItem>
                        )}
                    </NavLink>
                ))}
            </SidebarMenu>
        </SidebarGroup>
    );
}
