import { type LucideIcon } from 'lucide-react';

import {
    SidebarGroup,
    SidebarGroupLabel,
    SidebarMenu,
    SidebarMenuButton,
    SidebarMenuItem,
} from '@/components/ui/sidebar';
import { useAccessChecker } from '@/hooks/use-has-access';
import { NavLink } from 'react-router-dom';

type Props = {
    Workflows: {
        name: string;
        url: string;
        icon: LucideIcon;
        roles?: string[];
    }[];
    title?: string;
};

export function SimpleNavGroup({ Workflows, title }: Props) {
    const canAccess = useAccessChecker();

    const visibleWorkflows = Workflows.filter((item) => {
        return canAccess(item.roles);
    });

    if (visibleWorkflows.length === 0) {
        return null;
    }

    return (
        <SidebarGroup>
            {title !== null && <SidebarGroupLabel>{title}</SidebarGroupLabel>}
            <SidebarMenu className="font-semibold">
                {visibleWorkflows.map((item) => (
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
