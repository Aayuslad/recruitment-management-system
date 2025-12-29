'use client';

import { Check, Info, X } from 'lucide-react';
import * as React from 'react';

import { useGetUsersSummary } from '@/api/user-api';
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
import type { CreatePositionBatchCommandCorrected } from '@/types/position-types';
import { Label } from '../../ui/label';
import {
    Tooltip,
    TooltipContent,
    TooltipTrigger,
} from '@/components/ui/tooltip';

type Props = {
    fields: CreatePositionBatchCommandCorrected['reviewers'];
    append: (
        value: CreatePositionBatchCommandCorrected['reviewers'][0]
    ) => void;
    remove: (index: number) => void;
};

export function ReviewersSelector({ fields, append, remove }: Props) {
    const [open, setOpen] = React.useState(false);
    const { data, isLoading } = useGetUsersSummary();

    return (
        <div>
            <div className="flex justify-between">
                <h3 className="font-semibold text-lg flex items-center gap-1">
                    <Label>Reviewers</Label>
                    <span>
                        <Tooltip>
                            <TooltipTrigger asChild>
                                <Info className="w-4 h-4" />
                            </TooltipTrigger>
                            <TooltipContent>
                                <p className="text-wrap max-w-[200px] font-semibold">
                                    Selected reviewers will be assigned for
                                    screening of job applications for this
                                    position batch.
                                </p>
                            </TooltipContent>
                        </Tooltip>
                    </span>
                </h3>
                <Popover open={open} onOpenChange={setOpen}>
                    <PopoverTrigger asChild>
                        <Button
                            variant="ghost"
                            role="combobox"
                            aria-expanded={open}
                        >
                            + Add Reviewer
                        </Button>
                    </PopoverTrigger>
                    <PopoverContent className="w-[400px] p-0">
                        <Command>
                            <CommandInput
                                placeholder="Search user names..."
                                className="h-9"
                            />
                            <CommandList>
                                <CommandEmpty>No Users found.</CommandEmpty>
                                {isLoading && (
                                    <CommandEmpty>
                                        Loading Users...
                                    </CommandEmpty>
                                )}
                                <CommandGroup>
                                    {data?.map((userSummary) => (
                                        <CommandItem
                                            key={userSummary.userId}
                                            value={userSummary.userId}
                                            disabled={fields?.some(
                                                (x) =>
                                                    x.reviewerUserId ===
                                                    userSummary.userId
                                            )}
                                            onSelect={(currentValue) => {
                                                append({
                                                    reviewerUserId:
                                                        currentValue,
                                                });
                                                setOpen(false);
                                            }}
                                        >
                                            {userSummary.userName}
                                            <span className="text-muted-foreground">
                                                ({userSummary.email})
                                            </span>
                                            <Check
                                                className={cn(
                                                    'ml-auto',
                                                    fields?.some(
                                                        (x) =>
                                                            x.reviewerUserId ===
                                                            userSummary.userId
                                                    )
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
            </div>

            <div className="border rounded-2xl py-2 px-4">
                {fields?.map((field, index) => (
                    <div
                        key={field.reviewerUserId}
                        className="flex items-center justify-between"
                    >
                        <span className="space-x-2">
                            <span>
                                {data?.find(
                                    (x) => x.userId === field.reviewerUserId
                                )?.userName ?? '--'}
                            </span>
                            <span className="text-muted-foreground">
                                (
                                {data?.find(
                                    (x) => x.userId === field.reviewerUserId
                                )?.email ?? '--'}
                                )
                            </span>
                        </span>
                        <Button
                            size="sm"
                            variant="link"
                            onClick={() => remove(index)}
                            className="hover:cursor-pointer"
                        >
                            <X className="h-4 w-4" />
                        </Button>
                    </div>
                ))}
                {fields?.length === 0 && (
                    <div className="text-muted-foreground text-center py-3">
                        No reviewers selected.
                    </div>
                )}
            </div>
        </div>
    );
}
