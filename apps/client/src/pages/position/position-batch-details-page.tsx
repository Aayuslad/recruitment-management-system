import { useGetPositionBatch } from '@/api/position-api';
import { BatchPositionsTable } from '@/components/position/batch-positions-table';
import { DeletePositionBatchDialog } from '@/components/position/delete-position-batch-dialog';
import { EditPositionBatchSheet } from '@/components/position/edit-position-batch-sheet';
import { Avatar, AvatarFallback } from '@/components/ui/avatar';
import { Button } from '@/components/ui/button';
import {
    Item,
    ItemActions,
    ItemContent,
    ItemDescription,
    ItemGroup,
    ItemTitle,
} from '@/components/ui/item';
import { SIDEBAR_WIDTH } from '@/components/ui/sidebar';
import { SkillPill } from '@/components/ui/skill-pill';
import { Spinner } from '@/components/ui/spinner';
import {
    Tooltip,
    TooltipContent,
    TooltipTrigger,
} from '@/components/ui/tooltip';
import { useAppStore } from '@/store';
import { ExternalLink, Info } from 'lucide-react';
import React from 'react';
import { useNavigate, useParams } from 'react-router';
import { useShallow } from 'zustand/react/shallow';

export const PositionBatchDetailsPage = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
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
                    <EditPositionBatchSheet
                        positionBatchId={data.batchId}
                        visibleTo={['Admin', 'Recruiter']}
                    />
                    <DeletePositionBatchDialog
                        positionBatchId={data.batchId}
                        visibleTo={['Admin']}
                    />
                </div>
            </div>
            <div
                className="h-full flex mx-auto justify-center transition-width duration-200 ease-in-out"
                style={{
                    width: `calc(100vw - ${SIDEBAR_WIDTH} - ${sidebarState === 'opened' ? '80px' : '0px'})`,
                }}
            >
                <div className="flex-[50%] px-5 pt-8 space-y-7">
                    <div className="space-y-4">
                        <h3 className="font-semibold text-lg">Batch Details</h3>

                        <div className="space-y-2 text-sm">
                            {/* ID Row */}
                            {/* <div className="grid grid-cols-[110px_1fr_1fr] items-start gap-2">
                                <span className="text-muted-foreground">
                                    ID
                                </span>
                                <div className="font-mono">
                                    {data.batchId.slice(0, 6).toUpperCase()}...
                                    <button
                                        onClick={() => {
                                            navigator.clipboard.writeText(
                                                data.batchId
                                            );
                                            toast.success(
                                                'Copied to clipboard'
                                            );
                                        }}
                                        className="text-muted-foreground hover:text-foreground hover:cursor-pointer"
                                        title="Copy full ID"
                                    >
                                        <Copy size={16} />
                                    </button>
                                </div>
                            </div> */}

                            {/* Designation */}
                            <div className="grid grid-cols-[110px_1fr] items-start gap-2">
                                <span className="text-muted-foreground">
                                    Designation
                                </span>
                                <span
                                    className="hover:cursor-pointer hover:underline"
                                    onClick={(e) => {
                                        e.stopPropagation();
                                        navigate('/configuration/designations');
                                    }}
                                >
                                    {data.designationName}
                                </span>
                            </div>

                            {/* Location */}
                            <div className="grid grid-cols-[110px_1fr] items-start gap-2">
                                <span className=" text-muted-foreground">
                                    Location
                                </span>
                                <span className="">{data.jobLocation}</span>
                            </div>

                            {/* CTC */}
                            <div className="grid grid-cols-[110px_1fr] items-start gap-2">
                                <span className=" text-muted-foreground">
                                    CTC Range
                                </span>
                                <span className="">
                                    {data.minCTC} – {data.maxCTC} LPA
                                </span>
                            </div>

                            {/* Description */}
                            {data.description && (
                                <div className="grid grid-cols-[110px_1fr] items-start gap-2">
                                    <span className=" text-muted-foreground">
                                        Description
                                    </span>
                                    <span className="">{data.description}</span>
                                </div>
                            )}

                            <div className="grid grid-cols-[110px_1fr] items-start gap-2">
                                <span className=" text-muted-foreground">
                                    Creation
                                </span>
                                <span className="">
                                    By{' '}
                                    <span className="underline">
                                        {data.createdByUserName}
                                    </span>{' '}
                                    on{' '}
                                    <span className="font-">
                                        {new Date(
                                            data.createdAt
                                        ).toLocaleDateString()}
                                    </span>
                                </span>
                            </div>
                        </div>
                    </div>

                    <div className="space-y-2">
                        <h3 className="font-semibold text-lg">
                            Skills Criteria
                        </h3>
                        <div className="ml-1">
                            {data?.skills
                                .filter((x) => x.skillType === 'Required')
                                .map((x) => {
                                    return (
                                        <SkillPill
                                            id={x.skillId}
                                            name={x.skillName}
                                            type={x.skillType}
                                        />
                                    );
                                })}
                            {data?.skills
                                .filter((x) => x.skillType === 'Preferred')
                                .map((x) => {
                                    return (
                                        <SkillPill
                                            id={x.skillId}
                                            name={x.skillName}
                                            type={x.skillType}
                                        />
                                    );
                                })}
                        </div>
                    </div>

                    <div className="space-y-2 w-[450px]">
                        <div className="flex justify-between items-center">
                            <h3 className="font-semibold text-lg flex items-center gap-1">
                                <span>Reviewers</span>
                                <span>
                                    <Tooltip>
                                        <TooltipTrigger asChild>
                                            <Info className="w-4 h-4" />
                                        </TooltipTrigger>
                                        <TooltipContent>
                                            <p className="text-wrap max-w-[200px] font-semibold">
                                                Assigned Reviewers will be able
                                                to review job applications for
                                                this position batch
                                            </p>
                                        </TooltipContent>
                                    </Tooltip>
                                </span>
                            </h3>
                            <p className="text-sm">
                                {data.reviewers.length} Assigned
                            </p>
                        </div>
                        <div className="ml-1">
                            <div className="flex w-full  flex-col gap-6">
                                <ItemGroup>
                                    {data?.reviewers.map((person) => (
                                        <React.Fragment
                                            key={person.reviewerUserId}
                                        >
                                            <Item className="py-1.5 px-0 flex items-center border">
                                                <Avatar>
                                                    <AvatarFallback className="text-lg pb-1">
                                                        {person.reviewerUserName.charAt(
                                                            0
                                                        )}
                                                    </AvatarFallback>
                                                </Avatar>
                                                <ItemContent className="gap-0">
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
                                        </React.Fragment>
                                    ))}
                                </ItemGroup>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="flex-[50%] px-5 pt-8">
                    <h3 className="font-semibold text-lg">Positions:</h3>
                    <div className="w-full mt-3">
                        <div className="flex justify-around mb-3">
                            <div>
                                {data.positionsCount -
                                    data.closedPositionsCount}{' '}
                                Open
                            </div>
                            <div>{data.closedPositionsCount} Closed</div>
                            <div>{data.positionsOnHoldCount} On Hold</div>
                            <div>{data.positionsCount} Total</div>
                        </div>
                        <BatchPositionsTable batchId={id as string} />
                    </div>
                </div>
            </div>
        </div>
    );
};
