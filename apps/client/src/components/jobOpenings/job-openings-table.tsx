import {
    type ColumnDef,
    type ColumnFiltersState,
    flexRender,
    getCoreRowModel,
    getFilteredRowModel,
    getPaginationRowModel,
    getSortedRowModel,
    type SortingState,
    useReactTable,
    type VisibilityState,
} from '@tanstack/react-table';
import { ChevronDown, Copy } from 'lucide-react';
import * as React from 'react';

import { useGetJobOpenings } from '@/api/job-opening-api';
import { Button } from '@/components/ui/button';
import {
    DropdownMenu,
    DropdownMenuCheckboxItem,
    DropdownMenuContent,
    DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import { Input } from '@/components/ui/input';
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from '@/components/ui/table';
import type { JobOpeningSummary } from '@/types/job-opening-types';
import { useNavigate } from 'react-router-dom';
import { Spinner } from '../ui/spinner';
import { toast } from 'sonner';

export function JobOpeningsTable() {
    const navigate = useNavigate();
    const [sorting, setSorting] = React.useState<SortingState>([]);
    const [columnFilters, setColumnFilters] =
        React.useState<ColumnFiltersState>([]);
    const [columnVisibility, setColumnVisibility] =
        React.useState<VisibilityState>({});
    const [rowSelection, setRowSelection] = React.useState({});

    const { data, isLoading, isError } = useGetJobOpenings();

    const columns: ColumnDef<JobOpeningSummary>[] = [
        {
            accessorKey: 'title',
            header: () => <div className="w-[200px] ml-4">Title</div>,
            cell: ({ row }) => (
                <div className="font-medium pl-4">{row.original.title}</div>
            ),
        },
        {
            id: 'id',
            header: 'Opening Id',
            cell: ({ row }) => (
                <div className="font-medium w-[100px] relative group">
                    <span className="text-sm font-mono">
                        {row.original.id.slice(0, 6).toUpperCase()}...
                    </span>
                    <button
                        onClick={(e) => {
                            e.stopPropagation();
                            navigator.clipboard.writeText(row.original.id);
                            toast.success('Copied to clipboard');
                        }}
                        className="text-muted-foreground hover:text-foreground hover:cursor-pointer"
                        title="Copy full ID"
                    >
                        <Copy size={16} />
                    </button>
                </div>
            ),
        },
        {
            accessorKey: 'designationName',
            header: () => <div className="w-[150px] ml-4">Designation</div>,
            cell: ({ row }) => (
                <div
                    className="font-medium pl-4 hover:cursor-pointer hover:underline"
                    onClick={() => navigate('/configuration/designations')}
                >
                    {row.original.designationName}
                </div>
            ),
        },
        {
            id: 'type',
            header: 'Opening Type',
            cell: ({ row }) => (
                <div className="font-medium w-[130px]">{row.original.type}</div>
            ),
        },
        {
            id: 'interview-rounds',
            header: 'Interview Rounds',
            cell: ({ row }) => (
                <div className="font-medium w-[180px]">
                    {`${row.original.interviewRounds.map((r) => `#${r.roundNumber} ${r.type}`).join(', ')}` ||
                        '—'}
                </div>
            ),
        },
        {
            id: 'location',
            header: 'Location',
            cell: ({ row }) => (
                <div className="font-medium w-[150px]">
                    {row.original.jobLocation}
                </div>
            ),
        },
        {
            id: 'created-by',
            header: 'Created By',
            cell: ({ row }) => (
                <div className="font-medium w-[130px]">
                    {row.original.createdByUserName || '—'}
                </div>
            ),
        },
    ];

    const table = useReactTable({
        data: data as JobOpeningSummary[],
        columns,
        onSortingChange: setSorting,
        onColumnFiltersChange: setColumnFilters,
        getCoreRowModel: getCoreRowModel(),
        getPaginationRowModel: getPaginationRowModel(),
        getSortedRowModel: getSortedRowModel(),
        getFilteredRowModel: getFilteredRowModel(),
        onColumnVisibilityChange: setColumnVisibility,
        onRowSelectionChange: setRowSelection,
        state: {
            sorting,
            columnFilters,
            columnVisibility,
            rowSelection,
        },
    });

    if (isLoading)
        return (
            <div className="h-[50vh] flex justify-center items-center">
                <Spinner className="size-8" />
            </div>
        );
    if (isError) return <div>Error Loading Job Openings</div>;

    return (
        <div className="w-full">
            <h2 className="font-semibold text-xl">Job Openings:</h2>

            {/* header */}
            <div className="flex items-center justify-between py-4">
                <Input
                    placeholder="Filter job openings with title..."
                    value={
                        (table
                            .getColumn('title')
                            ?.getFilterValue() as string) ?? ''
                    }
                    onChange={(event) =>
                        table
                            .getColumn('title')
                            ?.setFilterValue(event.target.value)
                    }
                    className="max-w-sm"
                />
                <DropdownMenu>
                    <DropdownMenuTrigger asChild>
                        <Button variant="outline" className="ml-auto">
                            Columns <ChevronDown className="ml-2 h-4 w-4" />
                        </Button>
                    </DropdownMenuTrigger>
                    <DropdownMenuContent align="end">
                        {table
                            .getAllColumns()
                            .filter((column) => column.getCanHide())
                            .map((column) => (
                                <DropdownMenuCheckboxItem
                                    key={column.id}
                                    className="capitalize"
                                    checked={column.getIsVisible()}
                                    onCheckedChange={(value) =>
                                        column.toggleVisibility(!!value)
                                    }
                                >
                                    {column.id}
                                </DropdownMenuCheckboxItem>
                            ))}
                    </DropdownMenuContent>
                </DropdownMenu>
            </div>

            {/* table */}
            <div className="overflow-hidden rounded-md border">
                <Table>
                    <TableHeader>
                        {table.getHeaderGroups().map((headerGroup) => (
                            <TableRow key={headerGroup.id}>
                                {headerGroup.headers.map((header) => (
                                    <TableHead key={header.id}>
                                        {(() => {
                                            if (header.isPlaceholder) {
                                                return null;
                                            }
                                            return flexRender(
                                                header.column.columnDef.header,
                                                header.getContext()
                                            );
                                        })()}
                                    </TableHead>
                                ))}
                            </TableRow>
                        ))}
                    </TableHeader>
                    <TableBody>
                        {table.getRowModel().rows?.length ? (
                            table.getRowModel().rows.map((row) => (
                                <TableRow
                                    key={row.id}
                                    onClick={() =>
                                        navigate(
                                            `/job-openings/opening/${row.original.id}`
                                        )
                                    }
                                    data-state={
                                        row.getIsSelected() && 'selected'
                                    }
                                >
                                    {row.getVisibleCells().map((cell) => (
                                        <TableCell key={cell.id}>
                                            {flexRender(
                                                cell.column.columnDef.cell,
                                                cell.getContext()
                                            )}
                                        </TableCell>
                                    ))}
                                </TableRow>
                            ))
                        ) : (
                            <TableRow>
                                <TableCell
                                    colSpan={columns.length}
                                    className="h-24 text-center"
                                >
                                    No Job Openins Found.
                                </TableCell>
                            </TableRow>
                        )}
                    </TableBody>
                </Table>
            </div>

            {/* footer */}
            <div className="flex items-center justify-end space-x-2 py-4">
                <div className="space-x-2">
                    <Button
                        variant="outline"
                        size="sm"
                        onClick={() => table.previousPage()}
                        disabled={!table.getCanPreviousPage()}
                    >
                        Previous
                    </Button>
                    <Button
                        variant="outline"
                        size="sm"
                        onClick={() => table.nextPage()}
                        disabled={!table.getCanNextPage()}
                    >
                        Next
                    </Button>
                </div>
            </div>
        </div>
    );
}
