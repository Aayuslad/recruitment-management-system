import { useGetJobOpening } from '@/api/job-opening-api';
import { DeleteJobOpeningDialog } from '@/components/jobOpenings/delete-job-opening-dialog';
import { EditJobOpeningSheet } from '@/components/jobOpenings/edit-job-opening-sheet';
import { JobOpeningApplicationsTable } from '@/components/jobOpenings/job-opening-applications-table';
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
import { interviewParticipantRoleFormatConverter } from '@/util/interview-participant-role-format-converter';
import { durationFormatConverter } from '@/util/interview-round-duration-format-converter';
import { Copy, ExternalLink } from 'lucide-react';
import React from 'react';
import { useParams } from 'react-router';
import { toast } from 'sonner';
import { useShallow } from 'zustand/react/shallow';

export const JobOpeningDetailPage = () => {
    const { id } = useParams<{ id: string }>();
    const { sidebarState } = useAppStore(
        useShallow((s) => ({
            sidebarState: s.sidebarState,
        }))
    );

    const { data, isLoading, isError } = useGetJobOpening(id as string);

    if (!data || isLoading)
        return (
            <div className="w-full h-[80vh] flex items-center justify-center">
                <Spinner className="size-8" />
            </div>
        );

    if (isError)
        return (
            <div className="w-full h-[80vh] flex items-center justify-center">
                Error fetching job opening
            </div>
        );

    return (
        <div className="h-full flex flex-col mb-10">
            <div className="h-30 flex items-center px-10 border-b ">
                <div className="space-y-1">
                    <h1 className="text-2xl font-bold">Job Opening</h1>
                    <div className="space-x-3 font-semibold">
                        <span>{data.designationName}</span>
                        <span>(Ahmedabad)</span>
                    </div>
                </div>
                <div className="ml-auto mb-4 space-x-2">
                    <EditJobOpeningSheet jobOpeningId={data.id} />
                    <DeleteJobOpeningDialog id={data.id} />
                </div>
            </div>
            <div
                className="h-full flex mx-auto justify-center transition-width duration-200 ease-in-out"
                style={{
                    width: `calc(100vw - ${SIDEBAR_WIDTH} - ${sidebarState === 'opend' ? '80px' : '0px'})`,
                }}
            >
                <div className="flex-[50%] px-5 pt-8 space-y-7">
                    <div className="space-y-4">
                        <h3 className="font-semibold text-lg">
                            Job Opening Details
                        </h3>

                        <div className="space-y-2">
                            {/* ID Row */}
                            <div className="grid grid-cols-[110px_1fr_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    ID
                                </span>
                                <div className="text-sm font-mono">
                                    {data.id.slice(0, 6).toUpperCase()}...
                                    <button
                                        onClick={() => {
                                            navigator.clipboard.writeText(
                                                data.id
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
                            </div>

                            <div className="grid grid-cols-[110px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Title
                                </span>
                                <span className="text-sm">{data.title}</span>
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

                    <div className="space-y-4">
                        <h3 className="font-semibold text-lg">
                            Position Batch Details
                        </h3>

                        <div className="space-y-2">
                            {/* ID Row */}
                            <div className="grid grid-cols-[110px_1fr_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    ID
                                </span>
                                <div className="text-sm font-mono">
                                    {data.positionBatchId
                                        .slice(0, 6)
                                        .toUpperCase()}
                                    ...
                                    <button
                                        onClick={() => {
                                            navigator.clipboard.writeText(
                                                data.positionBatchId
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

                            {/* CTC */}
                            <div className="grid grid-cols-[110px_1fr] items-start gap-2">
                                <span className="text-sm text-muted-foreground">
                                    Positions Closed
                                </span>
                                <span className="text-sm">
                                    {data.closedPositionsCount} /{' '}
                                    {data.positionsCount}
                                </span>
                            </div>
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
                        <h3 className="font-semibold text-lg">
                            Interview Rounds
                        </h3>
                        <div className="ml-1 space-y-2">
                            {data.interviewRounds.map((round, key) => {
                                return (
                                    <div className="flex-1 px-1" key={key}>
                                        <h4 className="font-semibold space-x-2.5">
                                            <span>
                                                Round {round.roundNumber}
                                            </span>
                                            <span>-</span>
                                            <span>{round.type}</span>
                                        </h4>
                                        <div className="grid grid-cols-[140px_1fr] items-start gap-2">
                                            <span className="text-sm text-muted-foreground">
                                                Duration
                                            </span>
                                            <span className="text-sm">
                                                {durationFormatConverter(
                                                    round.durationInMinutes
                                                )}
                                            </span>
                                        </div>
                                        <div className="grid grid-cols-[140px_1fr] items-start gap-2">
                                            <span className="text-sm text-muted-foreground">
                                                Panel Requirements
                                            </span>
                                            <div className="text-sm">
                                                {round.requirements.map(
                                                    (requirement) => (
                                                        <span>
                                                            {interviewParticipantRoleFormatConverter(
                                                                requirement.role,
                                                                requirement.requirementCount
                                                            )}
                                                            ,{' '}
                                                        </span>
                                                    )
                                                )}
                                            </div>
                                        </div>
                                    </div>
                                );
                            })}
                        </div>
                    </div>

                    <div className="space-y-2">
                        <h3 className="font-semibold text-lg">Interviwers</h3>
                        <div className="ml-1">
                            <div className="flex w-full max-w-md flex-col gap-6">
                                <ItemGroup>
                                    {data.interviewers
                                        .reduce(
                                            (acc, i) => {
                                                const existing = acc.find(
                                                    (x) => x.userId === i.userId
                                                );

                                                if (!existing) {
                                                    acc.push({
                                                        userId: i.userId,
                                                        userName: i.userName,
                                                        email: i.email,
                                                        roles: [i.role],
                                                    });
                                                } else {
                                                    existing.roles.push(i.role);
                                                }

                                                return acc;
                                            },
                                            [] as {
                                                userId: string;
                                                userName: string;
                                                email: string;
                                                roles: string[];
                                            }[]
                                        )
                                        .map((person) => (
                                            <React.Fragment key={person.userId}>
                                                <Item className=" py-1.5 px-0">
                                                    <ItemMedia>
                                                        <Avatar>
                                                            {/* <AvatarImage
                                                            src={person.avatar}
                                                            className="grayscale"
                                                        /> */}
                                                            <AvatarFallback>
                                                                {person.userName.charAt(
                                                                    0
                                                                )}
                                                            </AvatarFallback>
                                                        </Avatar>
                                                    </ItemMedia>
                                                    <ItemContent className="gap-0">
                                                        <ItemTitle>
                                                            {person.userName}{' '}
                                                            <span className="text-muted-foreground">
                                                                ({person.email})
                                                            </span>
                                                        </ItemTitle>
                                                        <ItemDescription>
                                                            {person.roles.toString()}
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
                    <JobOpeningApplicationsTable jobOpeningId={data.id} />
                </div>
            </div>
        </div>
    );
};
