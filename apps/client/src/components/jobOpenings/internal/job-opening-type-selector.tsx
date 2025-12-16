'use client';

import { Check, ChevronsUpDown } from 'lucide-react';
import * as React from 'react';

import { Button } from '@/components/ui/button';
import {
    Command,
    CommandGroup,
    CommandItem,
    CommandList,
} from '@/components/ui/command';
import {
    Popover,
    PopoverContent,
    PopoverTrigger,
} from '@/components/ui/popover';
import { cn } from '@/lib/utils';
import { JOB_OPENING_TYPE, type JobOpeningType } from '@/types/enums';

const types: { value: JobOpeningType; label: string }[] = [
    {
        value: JOB_OPENING_TYPE.NORMAL,
        label: 'Normal Drive',
    },
    {
        value: JOB_OPENING_TYPE.CAMPUS_DRIVE,
        label: 'Campus Drive',
    },
    {
        value: JOB_OPENING_TYPE.WALK_IN,
        label: 'Walk In Drive',
    },
];

type Props = {
    selectedType: JobOpeningType | undefined;
    setSelectedType: (type: JobOpeningType) => void;
};

export function TypeSelector({ selectedType, setSelectedType }: Props) {
    const [open, setOpen] = React.useState(false);

    return (
        <Popover open={open} onOpenChange={setOpen}>
            <PopoverTrigger asChild>
                <Button
                    variant="outline"
                    role="combobox"
                    aria-expanded={open}
                    className="w-[300px] justify-between"
                >
                    {selectedType
                        ? types.find((type) => type.value === selectedType)
                              ?.label
                        : 'Select opening drive type...'}
                    <ChevronsUpDown className="opacity-50" />
                </Button>
            </PopoverTrigger>
            <PopoverContent className="w-[200px] p-0">
                <Command>
                    <CommandList>
                        <CommandGroup>
                            {types.map((framework) => (
                                <CommandItem
                                    key={framework.value}
                                    value={framework.value as JobOpeningType}
                                    onSelect={(currentValue) => {
                                        setSelectedType(
                                            currentValue as JobOpeningType
                                        );
                                        setOpen(false);
                                    }}
                                >
                                    {framework.label}
                                    <Check
                                        className={cn(
                                            'ml-auto',
                                            selectedType === framework.value
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
