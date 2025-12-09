import { useGetPositionBatch } from '@/api/position-api';
import { BatchPositionsTable } from '@/components/position/batch-positions-table';
import { DeletePositionBatchDialog } from '@/components/position/delete-position-batch-dialog';
import { EditPositionBatchSheet } from '@/components/position/edit-position-batch-sheet';
import { Avatar, AvatarFallback } from '@/components/ui/avatar';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import {
    Item,
    ItemActions,
    ItemContent,
    ItemDescription,
    ItemGroup,
    ItemMedia,
    ItemSeparator,
    ItemTitle,
} from '@/components/ui/item';
import { SIDEBAR_WIDTH } from '@/components/ui/sidebar';
import { Spinner } from '@/components/ui/spinner';
import {
    Tooltip,
    TooltipContent,
    TooltipTrigger,
} from '@/components/ui/tooltip';
import { useAppStore } from '@/store';
import { Copy, ExternalLink } from 'lucide-react';
import React from 'react';
import { useParams } from 'react-router';
import { toast } from 'sonner';
import { useShallow } from 'zustand/react/shallow';

export const PositionBatchDetailsPage = () => {
    const { id } = useParams<{ id: string }>();
    const { sidebarState } = useAppStore(
        useShallow((s) => ({
            sidebarState: s.sidebarState,
        }))
    );

    const { data, isLoading, isError } = useGetPositionBatch(id as string);

    if (!data || isLoading)
        return (
            <div className="w-full h-[80vh] flex items-center justify-center">
                <Spinner className="size-8" />
            </div>
        );

    if (isError)
        return (
            <div className="w-full h-[80vh] flex items-center justify-center">
                Error fetching position batch
            </div>
        );

    return (
        <div className="h-full flex flex-col mb-10">
            <div className="h-30 flex items-center px-10 border-b ">
                <div className="space-y-1">
                    <h1 className="text-2xl font-bold">Position Batch</h1>
                    <div className="space-x-3 font-semibold">
                        <span>{data.designationName}</span>
                        <span>({data.jobLocation})</span>
                    </div>
                </div>
                <div className="ml-auto mb-4 space-x-2">
                    <EditPositionBatchSheet positionBatchId={data.batchId} />
                    <DeletePositionBatchDialog batchId={data.batchId} />
                </div>
            </div>
            <div
                className="h-full flex mx-auto justify-center transition-width duration-200 ease-in-out"
                style={{
                    width: `calc(100vw - ${SIDEBAR_WIDTH} - ${sidebarState === 'opend' ? '80px' : '0px'})`,
                }}
            >
                <div className="flex-[55%] px-5 pt-8 space-y-7">
                    <div className="space-y-4">
                        <h3 className="font-semibold text-lg">Batch Details</h3>

                        <div className="space-y-2">
                            {/* ID Row */}
                            <div className="grid grid-cols-[110px_1fr_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    ID
                                </span>
                                <span className="text-sm font-mono">
                                    {data.batchId.slice(0, 6).toUpperCase()}...
                                </span>
                                <button
                                    onClick={() => {
                                        navigator.clipboard.writeText(
                                            data.batchId
                                        );
                                        toast.success('Copied to clipboard');
                                    }}
                                    className="text-muted-foreground hover:text-foreground hover:cursor-pointer"
                                    title="Copy full ID"
                                >
                                    <Copy size={16} />
                                </button>
                            </div>

                            {/* Designation */}
                            <div className="grid grid-cols-[110px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Designation
                                </span>
                                <span className="text-sm">
                                    {data.designationName}
                                </span>
                            </div>

                            {/* Location */}
                            <div className="grid grid-cols-[110px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Location
                                </span>
                                <span className="text-sm">
                                    {data.jobLocation}
                                </span>
                            </div>

                            {/* CTC */}
                            <div className="grid grid-cols-[110px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    CTC Range
                                </span>
                                <span className="text-sm">
                                    {data.minCTC} – {data.maxCTC} LPA
                                </span>
                            </div>

                            {/* Description */}
                            {data.description && (
                                <div className="grid grid-cols-[110px_1fr] items-start gap-2">
                                    <span className="text-sm text-muted-foreground">
                                        Description
                                    </span>
                                    <span className="text-sm">
                                        {data.description}
                                    </span>
                                </div>
                            )}
                        </div>
                    </div>

                    <div className="space-y-2">
                        <h3 className="font-semibold text-lg">Skills</h3>
                        <div className="ml-1">
                            {data?.skills
                                .filter((x) => x.skillType === 'Required')
                                .map((x) => {
                                    return (
                                        <Tooltip key={x.skillId}>
                                            <TooltipTrigger asChild>
                                                <Badge
                                                    variant="outline"
                                                    className="border-red-400 text-sm font-normal pb-1.5 px-2.5 mr-1 mb-1"
                                                >
                                                    <span>{x.skillName}</span>
                                                    {x.minExperienceYears !==
                                                        0 && (
                                                        <span className="text-xs -mb-1 pb-[1px] px-1.5 bg-accent rounded-2xl">
                                                            {
                                                                x.minExperienceYears
                                                            }
                                                        </span>
                                                    )}
                                                </Badge>
                                            </TooltipTrigger>
                                            <TooltipContent>
                                                <p className="space-x-1.5 mb-1">
                                                    <span className="font-semibold">
                                                        Type:
                                                    </span>
                                                    <span>{x.skillType}</span>
                                                </p>
                                                <p className="space-x-1.5">
                                                    <span className="font-semibold">
                                                        Minimum Experience
                                                        years:
                                                    </span>
                                                    <span>
                                                        {x.minExperienceYears}
                                                    </span>
                                                </p>
                                            </TooltipContent>
                                        </Tooltip>
                                    );
                                })}
                            {data?.skills
                                .filter((x) => x.skillType === 'Preferred')
                                .map((x) => {
                                    return (
                                        <Tooltip key={x.skillId}>
                                            <TooltipTrigger asChild>
                                                <Badge
                                                    variant="outline"
                                                    className="border-blue-400 text-sm font-normal pb-1.5 px-2.5 mr-1 mb-1"
                                                >
                                                    <span>{x.skillName}</span>
                                                    {x.minExperienceYears !==
                                                        0 && (
                                                        <span className="text-xs -mb-1 pb-[1px] px-1.5 bg-accent rounded-2xl">
                                                            {
                                                                x.minExperienceYears
                                                            }
                                                        </span>
                                                    )}
                                                </Badge>
                                            </TooltipTrigger>
                                            <TooltipContent>
                                                <p className="space-x-1.5 mb-1">
                                                    <span className="font-semibold">
                                                        Type:
                                                    </span>
                                                    <span>{x.skillType}</span>
                                                </p>
                                                <p className="space-x-1.5">
                                                    <span className="font-semibold">
                                                        Minimum Experience
                                                        years:
                                                    </span>
                                                    <span>
                                                        {x.minExperienceYears}
                                                    </span>
                                                </p>
                                            </TooltipContent>
                                        </Tooltip>
                                    );
                                })}
                            {data?.skills
                                .filter((x) => x.skillType === 'NiceToHave')
                                .map((x) => {
                                    return (
                                        <Tooltip key={x.skillId}>
                                            <TooltipTrigger asChild>
                                                <Badge
                                                    variant="outline"
                                                    className="border-slate-400 text-sm font-normal pb-1.5 px-2.5 mr-1 mb-1"
                                                >
                                                    <span>{x.skillName}</span>
                                                    {x.minExperienceYears !==
                                                        0 && (
                                                        <span className="text-xs -mb-1 pb-[1px] px-1.5 bg-accent rounded-2xl">
                                                            {
                                                                x.minExperienceYears
                                                            }
                                                        </span>
                                                    )}
                                                </Badge>
                                            </TooltipTrigger>
                                            <TooltipContent>
                                                <p className="space-x-1.5 mb-1">
                                                    <span className="font-semibold">
                                                        Type:
                                                    </span>
                                                    <span>{x.skillType}</span>
                                                </p>
                                                <p className="space-x-1.5">
                                                    <span className="font-semibold">
                                                        Minimum Experience
                                                        years:
                                                    </span>
                                                    <span>
                                                        {x.minExperienceYears}
                                                    </span>
                                                </p>
                                            </TooltipContent>
                                        </Tooltip>
                                    );
                                })}
                        </div>
                    </div>

                    <div className="space-y-2">
                        <h3 className="font-semibold text-lg">Reviewers</h3>
                        <div className="ml-1">
                            <div className="flex w-full max-w-md flex-col gap-6">
                                <ItemGroup>
                                    {data?.reviewers.map((person, index) => (
                                        <React.Fragment
                                            key={person.reviewerUserId}
                                        >
                                            <Item>
                                                <ItemMedia>
                                                    <Avatar>
                                                        {/* <AvatarImage
                                                            src={person.avatar}
                                                            className="grayscale"
                                                        /> */}
                                                        <AvatarFallback>
                                                            {person.reviewerUserName.charAt(
                                                                0
                                                            )}
                                                        </AvatarFallback>
                                                    </Avatar>
                                                </ItemMedia>
                                                <ItemContent className="gap-1">
                                                    <ItemTitle>
                                                        {
                                                            person.reviewerUserName
                                                        }
                                                    </ItemTitle>
                                                    <ItemDescription>
                                                        {
                                                            person.reviewerUserEmail
                                                        }
                                                    </ItemDescription>
                                                </ItemContent>
                                                <ItemActions>
                                                    <Button
                                                        variant="ghost"
                                                        size="icon"
                                                        className="rounded-full"
                                                    >
                                                        <ExternalLink />
                                                    </Button>
                                                </ItemActions>
                                            </Item>
                                            {index !==
                                                data?.reviewers.length - 1 && (
                                                <ItemSeparator />
                                            )}
                                        </React.Fragment>
                                    ))}
                                </ItemGroup>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="flex-[45%] px-5 pt-8">
                    <h3 className="font-semibold text-lg">Positions:</h3>
                    <div className="w-full mt-4">
                        <BatchPositionsTable batchId={id as string} />
                    </div>
                </div>
            </div>
        </div>
    );
};
