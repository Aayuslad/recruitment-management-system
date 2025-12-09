'use client';

import { Check, X } from 'lucide-react';
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

type Props = {
    fealds: CreatePositionBatchCommandCorrected['reviewers'];
    append: (
        value: CreatePositionBatchCommandCorrected['reviewers'][0]
    ) => void;
    remove: (index: number) => void;
};

export function ReviewersSelector({ fealds, append, remove }: Props) {
    const [open, setOpen] = React.useState(false);
    const { data, isLoading } = useGetUsersSummary();

    return (
        <div>
            <div className="flex justify-between">
                <Label htmlFor="reviewers">Reviewers</Label>
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
                                            disabled={fealds?.some(
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
                                                    fealds?.some(
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
                {fealds?.map((feald, index) => (
                    <div
                        key={feald.reviewerUserId}
                        className="flex items-center justify-between"
                    >
                        <span className="space-x-2">
                            <span>
                                {data?.find(
                                    (x) => x.userId === feald.reviewerUserId
                                )?.userName ?? '--'}
                            </span>
                            <span className="text-muted-foreground">
                                (
                                {data?.find(
                                    (x) => x.userId === feald.reviewerUserId
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
                {fealds?.length === 0 && (
                    <div className="text-muted-foreground text-center py-3">
                        No reviewers selected.
                    </div>
                )}
            </div>
        </div>
    );
}
