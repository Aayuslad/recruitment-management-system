'use client';

import { Check } from 'lucide-react';
import * as React from 'react';

import { useGetRoles } from '@/api/role-api';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import {
    Command,
    CommandEmpty,
    CommandGroup,
    CommandInput,
    CommandItem,
    CommandList,
} from '@/components/ui/command';
import {
    Popover,
    PopoverContent,
    PopoverTrigger,
} from '@/components/ui/popover';
import { cn } from '@/lib/utils';
import type { EditUserRolesCommandCorrected } from '@/types/user-types';
import { Label } from 'react-aria-components';

type Props = {
    userRoles: EditUserRolesCommandCorrected['roles'];
    appendedRoles: (role: EditUserRolesCommandCorrected['roles'][0]) => void;
    removedRoles: (index: number) => void;
};

export function UserRolesSelector({
    userRoles,
    appendedRoles,
    removedRoles,
}: Props) {
    const [open, setOpen] = React.useState(false);
    const { data: roles, isLoading, isError } = useGetRoles();

    if (!roles && isLoading) {
        return <div>Loading roles...</div>;
    }

    if (isError) {
        return <div>Error loading roles</div>;
    }

    return (
        <div>
            <div className="flex items-center justify-between">
                <Label>Roles</Label>

                <Popover open={open} onOpenChange={setOpen}>
                    <PopoverTrigger asChild>
                        <Button
                            variant="ghost"
                            role="combobox"
                            aria-expanded={open}
                            className="justify-between"
                        >
                            Select Roles
                        </Button>
                    </PopoverTrigger>
                    <PopoverContent className="w-[200px] p-0">
                        <Command>
                            <CommandInput
                                placeholder="Search roles..."
                                className="h-9"
                            />
                            <CommandList>
                                <CommandEmpty>No roles found.</CommandEmpty>
                                <CommandGroup>
                                    {roles?.map((role) => {
                                        if (role.name === 'Admin') {
                                            return null;
                                        }

                                        return (
                                            <CommandItem
                                                key={role.id}
                                                value={role.name}
                                                onSelect={() => {
                                                    const exists =
                                                        userRoles.find(
                                                            (x) =>
                                                                x.roleId ===
                                                                role.id
                                                        );

                                                    if (exists) {
                                                        const index =
                                                            userRoles.findIndex(
                                                                (x) =>
                                                                    x.roleId ===
                                                                    role.id
                                                            );
                                                        removedRoles(index);
                                                    } else {
                                                        appendedRoles({
                                                            roleId: role.id,
                                                            assignedBy: null,
                                                        });
                                                    }
                                                }}
                                            >
                                                {role.name}
                                                <Check
                                                    className={cn(
                                                        'ml-auto',
                                                        userRoles.some(
                                                            (x) =>
                                                                x.roleId ===
                                                                role.id
                                                        )
                                                            ? 'opacity-100'
                                                            : 'opacity-0'
                                                    )}
                                                />
                                            </CommandItem>
                                        );
                                    })}
                                </CommandGroup>
                            </CommandList>
                        </Command>
                    </PopoverContent>
                </Popover>
            </div>

            <div className="mt-2 border py-3 px-3 rounded-xl space-y-2">
                {userRoles.map((userRole) => {
                    const role = roles?.find((r) => r.id === userRole.roleId);
                    return (
                        <div
                            key={userRole.roleId}
                            className="flex items-center justify-between"
                        >
                            <Badge variant="secondary" className="mr-2 text-sm">
                                {role?.name}
                            </Badge>

                            <Button
                                variant="ghost"
                                size="sm"
                                disabled={role?.name === 'Admin'}
                                onClick={() => {
                                    const index = userRoles.findIndex(
                                        (x) => x.roleId === userRole.roleId
                                    );
                                    removedRoles(index);
                                }}
                            >
                                Remove
                            </Button>
                        </div>
                    );
                })}
                {userRoles.length === 0 && (
                    <div className="text-sm text-center py-1 text-muted-foreground">
                        No roles assigned.
                    </div>
                )}
            </div>
        </div>
    );
}
