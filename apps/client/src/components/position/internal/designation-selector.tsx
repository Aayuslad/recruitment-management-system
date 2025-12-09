/* eslint-disable indent */
'use client';

import { Check, ChevronsUpDown } from 'lucide-react';
import * as React from 'react';

import { useGetDesignations } from '@/api/designation-api';
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
import type { Designation } from '@/types/designation-types';

type Props = {
    setSelectedDesignationId: (id: string) => void;
};

export function DesignationSelector({ setSelectedDesignationId }: Props) {
    const [open, setOpen] = React.useState(false);
    const [selectedDesignation, setSelectedDesignation] =
        React.useState<Designation>();
    const { data, isLoading } = useGetDesignations();

    React.useEffect(() => {
        if (selectedDesignation) {
            setSelectedDesignationId(selectedDesignation.id);
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [selectedDesignation]);

    return (
        <Popover open={open} onOpenChange={setOpen}>
            <PopoverTrigger asChild>
                <Button
                    variant="outline"
                    role="combobox"
                    aria-expanded={open}
                    className="w-[300px] justify-between"
                >
                    {selectedDesignation
                        ? data?.find(
                              (designation) =>
                                  designation.name === selectedDesignation.name
                          )?.name
                        : 'Select Designation...'}
                    <ChevronsUpDown className="opacity-50" />
                </Button>
            </PopoverTrigger>
            <PopoverContent className="w-[250px] p-0">
                <Command>
                    <CommandInput
                        placeholder="Search designation..."
                        className="h-9"
                    />
                    <CommandList>
                        <CommandEmpty>No Designation found.</CommandEmpty>
                        {isLoading && (
                            <CommandEmpty>Loading Designations...</CommandEmpty>
                        )}
                        <CommandGroup>
                            {data?.map((designation) => (
                                <CommandItem
                                    key={designation.id}
                                    value={designation.name}
                                    onSelect={(currentValue) => {
                                        setSelectedDesignation(
                                            data.find(
                                                (designation) =>
                                                    designation.name ===
                                                    currentValue
                                            )
                                        );
                                        setOpen(false);
                                    }}
                                >
                                    {designation.name}
                                    <Check
                                        className={cn(
                                            'ml-auto',
                                            selectedDesignation?.id ===
                                                designation.id
                                                ? 'opacity-100'
                                                : 'opacity-0'
                                        )}
                                    />
                                </CommandItem>
                            ))}
                        </CommandGroup>
                    </CommandList>
                </Command>
            </PopoverContent>
        </Popover>
    );
}
