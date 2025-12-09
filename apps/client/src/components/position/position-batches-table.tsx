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

import { useGetPositionBatches } from '@/api/position-api';
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
import type { PositionBatchSummary } from '@/types/position-types';
import { useNavigate } from 'react-router-dom';
import { toast } from 'sonner';
import { Spinner } from '../ui/spinner';

export function PositionBatchesTable() {
    const navigate = useNavigate();
    const [sorting, setSorting] = React.useState<SortingState>([]);
    const [columnFilters, setColumnFilters] =
        React.useState<ColumnFiltersState>([]);
    const [columnVisibility, setColumnVisibility] =
        React.useState<VisibilityState>({});
    const [rowSelection, setRowSelection] = React.useState({});

    const { data, isLoading, isError } = useGetPositionBatches();

    const columns: ColumnDef<PositionBatchSummary>[] = [
        {
            accessorKey: 'designationName',
            header: () => <div className="w-[200px] ml-4">Designation</div>,
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
            id: 'Batch Id',
            header: 'Batch Id',
            cell: ({ row }) => (
                <div className="font-medium w-[150px] relative group">
                    <span className="text-sm font-mono">
                        {row.original.batchId.slice(0, 6).toUpperCase()}...
                    </span>
                    <button
                        onClick={(e) => {
                            e.stopPropagation();
                            navigator.clipboard.writeText(row.original.batchId);
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
            id: 'location',
            header: 'Location',
            cell: ({ row }) => (
                <div className="font-medium w-[180px]">
                    {row.original.jobLocation}
                </div>
            ),
        },
        {
            id: 'ctc-range',
            header: 'CTC Range',
            cell: ({ row }) => (
                <div className="font-medium w-[100px]">
                    {row.original.minCTC} - {row.original.maxCTC} LPA
                </div>
            ),
        },
        {
            id: 'positions',
            header: 'Positions',
            cell: ({ row }) => (
                <div className="font-medium w-[80px]">
                    {row.original.positionsCount}
                </div>
            ),
        },
        {
            id: 'closed-positions',
            header: 'Closed',
            cell: ({ row }) => (
                <div className="font-medium w-[80px]">
                    {row.original.closedPositionsCount}
                </div>
            ),
        },
        {
            id: 'on-hold-positions',
            header: 'On Hold',
            cell: ({ row }) => (
                <div className="font-medium w-[80px]">
                    {row.original.positionsOnHoldCount}
                </div>
            ),
        },
        {
            id: 'created-by',
            header: 'Created By',
            cell: ({ row }) => (
                <div className="font-medium w-[150px]">
                    {row.original.createdByUserName || '—'}
                </div>
            ),
        },
    ];

    const table = useReactTable({
        data: data as PositionBatchSummary[],
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
    if (isError) return <div>Error Loading Position Batches</div>;

    return (
        <div className="w-full">
            <h2 className="font-semibold text-xl">Position Batches:</h2>

            {/* header */}
            <div className="flex items-center py-4">
                <Input
                    placeholder="Filter position Batches with designation..."
                    value={
                        (table
                            .getColumn('designationName')
                            ?.getFilterValue() as string) ?? ''
                    }
                    onChange={(event) =>
                        table
                            .getColumn('designationName')
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
                                            `/positions/batch/${row.original.batchId}`
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
                                    No Position Batches Found.
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
