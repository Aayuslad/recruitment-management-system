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

const durations = [
    {
        value: 30,
        label: '30 Minutes',
    },
    {
        value: 45,
        label: '45 Minutes',
    },
    {
        value: 60,
        label: '1 Hour',
    },
    {
        value: 75,
        label: '1 Hour 15 Minutes',
    },
    {
        value: 90,
        label: '1 Hour 30 Minutes',
    },
    {
        value: 105,
        label: '1 Hour 45 Minutes',
    },
    {
        value: 120,
        label: '2 Hour',
    },
];

type Props = {
    selectedDuration: number | undefined;
    setSelectedDuration: (duration: number) => void;
};

export function InterviewRoundDurationSelector({
    selectedDuration,
    setSelectedDuration,
}: Props) {
    const [open, setOpen] = React.useState(false);

    return (
        <Popover open={open} onOpenChange={setOpen}>
            <PopoverTrigger asChild>
                <Button
                    variant="outline"
                    role="combobox"
                    aria-expanded={open}
                    className="w-[170px] h-7 justify-between"
                >
                    {selectedDuration
                        ? durations.find(
                              (duration) => duration.value === selectedDuration
                          )?.label
                        : 'Select Duration...'}
                    <ChevronsUpDown className="opacity-50" />
                </Button>
            </PopoverTrigger>
            <PopoverContent className="w-[200px] p-0">
                <Command>
                    <CommandList>
                        <CommandGroup>
                            {durations.map((duration) => (
                                <CommandItem
                                    key={duration.value}
                                    value={duration.value.toString()}
                                    onSelect={(currentValue) => {
                                        setSelectedDuration(
                                            currentValue ===
                                                selectedDuration?.toString()
                                                ? 0
                                                : Number(currentValue)
                                        );
                                        setOpen(false);
                                    }}
                                >
                                    {duration.label}
                                    <Check
                                        className={cn(
                                            'ml-auto',
                                            selectedDuration === duration.value
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
